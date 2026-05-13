using Microsoft.AspNetCore.Authorization;
using Ofix.CarListingImages;
using Ofix.Models;
using Ofix.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Ofix.CarListings
{
    [Authorize(OfixPermissions.CarListings.Default)]
    public class CarListingAppService : ApplicationService, ICarListingAppService
    {
        private readonly IRepository<CarListing, Guid> _repository;

        public CarListingAppService(IRepository<CarListing, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<CarListingDto> GetAsync(Guid id)
        {
            var queryable = await _repository.WithDetailsAsync(
                x => x.Brand,
                x => x.Model,
                x => x.SubModel,
                x => x.Images
            );

            var carListing = await AsyncExecuter.FirstOrDefaultAsync(queryable.Where(x => x.Id == id));

            if (carListing == null)
            {
                throw new Exception("Car listing not found.");
            }

            return MapToCarListingDto(carListing, includeAllImages: false);
        }

        [AllowAnonymous]
        public virtual async Task<CarListingDto> GetPublishedDetailAsync(Guid id)
        {
            var queryable = await _repository.WithDetailsAsync(
                x => x.Brand,
                x => x.Model,
                x => x.SubModel,
                x => x.Images
            );

            var carListing = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Where(x => x.Id == id && x.ListingStatus == ListingStatus.Active));

            if (carListing == null)
            {
                throw new EntityNotFoundException(typeof(CarListing), id);
            }

            return MapToCarListingDto(carListing, includeAllImages: true);
        }

        [AllowAnonymous]
        public virtual async Task<PagedResultDto<CarListingDto>> GetPublishedListAsync(CarListingListInput input)
        {
            input.ListingStatus = ListingStatus.Active;
            return await GetListAsync(input);
        }

        public async Task<PagedResultDto<CarListingDto>> GetListAsync(CarListingListInput input)
        {
            var queryable = await _repository.WithDetailsAsync(
                x => x.Brand,
                x => x.Model,
                x => x.SubModel,
                x => x.Images
            );

            var filteredQuery = queryable
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title.Contains(input.Title!))
                .WhereIf(input.BrandId.HasValue, x => x.BrandId == input.BrandId)
                .WhereIf(input.ModelId.HasValue, x => x.ModelId == input.ModelId)
                .WhereIf(input.SubModelId.HasValue, x => x.SubModelId == input.SubModelId.Value)
                .WhereIf(input.ListingStatus.HasValue, x => x.ListingStatus == input.ListingStatus.Value)
                .WhereIf(input.MinPrice.HasValue, x => x.Price >= input.MinPrice.Value)
                .WhereIf(input.MaxPrice.HasValue, x => x.Price <= input.MaxPrice.Value)
                .WhereIf(input.MinMileage.HasValue, x => x.Mileage >= input.MinMileage.Value)
                .WhereIf(input.MaxMileage.HasValue, x => x.Mileage <= input.MaxMileage.Value)
                .WhereIf(input.MinYear.HasValue, x => x.Year >= input.MinYear.Value)
                .WhereIf(input.MaxYear.HasValue, x => x.Year <= input.MaxYear.Value)
                .WhereIf(input.FuelType.HasValue, x => x.FuelType == input.FuelType.Value)
                .WhereIf(input.Transmission.HasValue, x => x.Transmission == input.Transmission.Value)
                .WhereIf(input.BodyShape.HasValue, x => x.BodyShape == input.BodyShape.Value);

            var totalCount = await AsyncExecuter.CountAsync(filteredQuery);

            var carListings = await AsyncExecuter.ToListAsync(
                filteredQuery
                    .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Title" : input.Sorting)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var items = carListings.Select(x => MapToCarListingDto(x, includeAllImages: false)).ToList();

            return new PagedResultDto<CarListingDto>(totalCount, items);
        }

        [Authorize(OfixPermissions.CarListings.Create)]
        public async Task<CarListingDto> CreateAsync(CreateUpdateCarListingDto input)
        {
            var carListing = new CarListing
            {
                BrandId = input.BrandId,
                ModelId = input.ModelId,
                SubModelId = input.SubModelId!.Value,
                Title = input.Title,
                Price = input.Price,
                Year = input.Year,
                Mileage = input.Mileage,
                ListingStatus = input.ListingStatus!.Value,
                Description = input.Description,
                Transmission = input.Transmission!.Value,
                FuelType = input.FuelType!.Value,
                BodyShape = input.BodyShape!.Value
            };

            await _repository.InsertAsync(carListing, autoSave: true);

            var createdQuery = await _repository.WithDetailsAsync(
                x => x.Brand,
                x => x.Model,
                x => x.SubModel,
                x => x.Images
            );

            var createdEntity = await AsyncExecuter.FirstOrDefaultAsync(createdQuery.Where(x => x.Id == carListing.Id));

            return MapToCarListingDto(createdEntity!, includeAllImages: false);
        }

        [Authorize(OfixPermissions.CarListings.Edit)]
        public async Task<CarListingDto> UpdateAsync(Guid id, CreateUpdateCarListingDto input)
        {
            var carListing = await _repository.GetAsync(id);

            carListing.BrandId = input.BrandId;
            carListing.ModelId = input.ModelId;
            carListing.SubModelId = input.SubModelId!.Value;
            carListing.Title = input.Title;
            carListing.Price = input.Price;
            carListing.Year = input.Year;
            carListing.Mileage = input.Mileage;
            carListing.ListingStatus = input.ListingStatus!.Value;
            carListing.Description = input.Description;
            carListing.Transmission = input.Transmission!.Value;
            carListing.FuelType = input.FuelType!.Value;
            carListing.BodyShape = input.BodyShape!.Value;

            await _repository.UpdateAsync(carListing, autoSave: true);

            var updatedQuery = await _repository.WithDetailsAsync(
                x => x.Brand,
                x => x.Model,
                x => x.SubModel,
                x => x.Images
            );

            var updatedEntity = await AsyncExecuter.FirstOrDefaultAsync(updatedQuery.Where(x => x.Id == id));

            return MapToCarListingDto(updatedEntity!, includeAllImages: false);
        }

        [Authorize(OfixPermissions.CarListings.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        private CarListingDto MapToCarListingDto(CarListing carListing, bool includeAllImages)
        {
            var imageVolgorde = carListing.Images != null
                ? carListing.Images.OrderBy(x => x.SortOrder).ToList()
                : new List<CarListingImage>();

            return new CarListingDto
            {
                Id = carListing.Id,
                Title = carListing.Title,
                BrandId = carListing.BrandId,
                BrandName = carListing.Brand?.Name,
                ModelId = carListing.ModelId,
                ModelName = carListing.Model?.Name,
                SubModelId = carListing.SubModelId,
                SubModelName = carListing.SubModel?.Name,
                Price = carListing.Price,
                Year = carListing.Year,
                Mileage = carListing.Mileage,
                ListingStatus = carListing.ListingStatus,
                Transmission = carListing.Transmission,
                FuelType = carListing.FuelType,
                BodyShape = carListing.BodyShape,
                Description = carListing.Description,
                CreationTime = carListing.CreationTime,
                CoverImageUrl = imageVolgorde
                    .FirstOrDefault(x => x.IsCover)?.BlobName
                    ?? imageVolgorde.FirstOrDefault()?.BlobName,
                Images = includeAllImages
                    ? imageVolgorde.Select(MapNaarCarListingImageDto).ToList()
                    : new List<CarListingImageDto>()
            };
        }

        private static CarListingImageDto MapNaarCarListingImageDto(CarListingImage afbeelding)
        {
            return new CarListingImageDto
            {
                Id = afbeelding.Id,
                FileName = afbeelding.FileName,
                ContentType = afbeelding.ContentType,
                FileSize = afbeelding.FileSize,
                Url = afbeelding.BlobName,
                SortOrder = afbeelding.SortOrder,
                IsCover = afbeelding.IsCover,
                CreationTime = afbeelding.CreationTime,
                CreatorId = afbeelding.CreatorId,
                LastModificationTime = afbeelding.LastModificationTime,
                LastModifierId = afbeelding.LastModifierId
            };
        }
    }
}