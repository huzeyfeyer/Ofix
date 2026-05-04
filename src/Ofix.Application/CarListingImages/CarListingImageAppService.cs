using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Ofix.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Microsoft.Extensions.Logging;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace Ofix.CarListingImages
{
    [Authorize(OfixPermissions.CarListingImages.Default)]
    public class CarListingImageAppService : ApplicationService, ICarListingImageAppService
    {
        private readonly IRepository<CarListingImage, Guid> _repository;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly Microsoft.Extensions.Logging.ILogger<CarListingImageAppService> _logger;

        private string WebRootPath => Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot");

        public CarListingImageAppService(
            IRepository<CarListingImage, Guid> repository,
            IHostEnvironment hostEnvironment,
            Microsoft.Extensions.Logging.ILogger<CarListingImageAppService> logger)
        {
            _repository = repository;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public async Task<List<CarListingImageDto>> GetListAsync(Guid carListingId)
        {
            var queryable = await _repository.GetQueryableAsync();

            var images = await AsyncExecuter.ToListAsync(
                queryable
                    .Where(x => x.CarListingId == carListingId)
                    .OrderBy(x => x.SortOrder)
            );

            return images.Select(x => MapToCarListingImageDto(x)).ToList();
        }

        [Authorize(OfixPermissions.CarListingImages.Edit)]
        public async Task SaveImagesAsync(Guid carListingId, List<SaveCarListingImageInput> images)
        {
            var queryable = await _repository.GetQueryableAsync();

            var existingImages = await _repository.GetListAsync(
                x => x.CarListingId == carListingId
                );

            foreach (var input in images)
            {
                
                if (input.ExistingImageId.HasValue)
                {
                    var existingImage = existingImages.FirstOrDefault(x => x.Id == input.ExistingImageId.Value);

                    if (existingImage == null)
                        continue;

                    if (input.IsDeleted)
                    {
                        await _repository.DeleteAsync(existingImage.Id);
                        DeletePermanentFileIfExists(existingImage.BlobName);
                        continue;
                    }

                    existingImage.SortOrder = input.SortOrder;
                    existingImage.IsCover = input.IsCover;

                    await _repository.UpdateAsync(existingImage, autoSave: true);
                    continue;
                }

               
                if (!string.IsNullOrWhiteSpace(input.TempFileToken) && !input.IsDeleted)
                {
                    var tempFolder = Path.Combine(
                        WebRootPath,
                        "uploads",
                        "temp",
                        "car-listings"
                    );

                    var tempFilePath = FindTempFilePath(input.TempFileToken);

                    if (string.IsNullOrWhiteSpace(tempFilePath) || !File.Exists(tempFilePath))
                    {
                        _logger.LogWarning("Temp file not found for token {Token} (carListingId={CarListingId})", input.TempFileToken, carListingId);
                        continue;
                    }

                    var originalFileName = Path.GetFileName(tempFilePath);
                    var extension = Path.GetExtension(tempFilePath);

                    var permanentFolder = Path.Combine(
                        WebRootPath,
                        "uploads",
                        "car-listings"
                    );

                    if (!Directory.Exists(permanentFolder))
                        Directory.CreateDirectory(permanentFolder);

                    var permanentFileName = Guid.NewGuid().ToString("N") + extension;
                    var permanentFullPath = Path.Combine(permanentFolder, permanentFileName);

                    try
                    {
                        File.Move(tempFilePath, permanentFullPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to move temp file {TempPath} to {PermanentPath}", tempFilePath, permanentFullPath);
                        // skip this file but continue processing others
                        continue;
                    }

                    var fileInfo = new FileInfo(permanentFullPath);

                    var image = new CarListingImage
                    {
                        CarListingId = carListingId,
                        FileName = originalFileName,
                        ContentType = GetContentTypeByExtension(extension),
                        FileSize = fileInfo.Length,
                        BlobName = $"/uploads/car-listings/{permanentFileName}",
                        SortOrder = input.SortOrder,
                        IsCover = input.IsCover
                    };

                    await _repository.InsertAsync(image, autoSave: true);
                    _logger.LogInformation("Saved image for carListingId={CarListingId}: {BlobName}", carListingId, image.BlobName);
                }
            }

            
            await EnsureSingleCoverAsync(carListingId);
        }



        private CarListingImageDto MapToCarListingImageDto(CarListingImage image)
        {
            return new CarListingImageDto
            {
                Id = image.Id,
                FileName = image.FileName,
                ContentType = image.ContentType,
                FileSize = image.FileSize,
                Url = image.BlobName,
                SortOrder = image.SortOrder,
                IsCover = image.IsCover
            };
        }

        private string? FindTempFilePath(string tempFileToken)
        {
            var tempFolder = Path.Combine(
                WebRootPath,
                "uploads",
                "temp",
                "car-listings"
            );

            if (!Directory.Exists(tempFolder))
            {
                return null;
            }

            try
            {
                return Directory.GetFiles(tempFolder, tempFileToken + ".*").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding temp file for token {Token}", tempFileToken);
                return null;
            }
        }

        private void DeletePermanentFileIfExists(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return;
            }

            var normalizedPath = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(WebRootPath, normalizedPath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private async Task EnsureSingleCoverAsync(Guid carListingId)
        {
            var images = await _repository.GetListAsync(x => x.CarListingId == carListingId);

            if (!images.Any())
            {
                return;
            }

            var coverImages = images.Where(x => x.IsCover).OrderBy(x => x.SortOrder).ToList();

            if (coverImages.Count == 0)
            {
                var firstImage = images.OrderBy(x => x.SortOrder).First();
                firstImage.IsCover = true;
                await _repository.UpdateAsync(firstImage, autoSave: true);
                return;
            }

            var selectedCoverId = coverImages.First().Id;

            foreach (var image in images)
            {
                var shouldBeCover = image.Id == selectedCoverId;

                if (image.IsCover != shouldBeCover)
                {
                    image.IsCover = shouldBeCover;
                    await _repository.UpdateAsync(image, autoSave: true);
                }
            }
        }

        private string GetContentTypeByExtension(string? extension)
        {
            return extension?.ToLowerInvariant() switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
