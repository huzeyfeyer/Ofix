using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Ofix.Files;
using Ofix.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace Ofix.Brands;

[Authorize(OfixPermissions.Brands.Default)]
public class BrandAppService : ApplicationService, IBrandAppService
{
    private readonly IRepository<Brand, Guid> _repository;
    private readonly IBlobContainer<BrandImageContainer> _brandImageContainer;

    public BrandAppService(IRepository<Brand, Guid> repository, IBlobContainer<BrandImageContainer> brandImageContainer)
    {
        _repository = repository;
        _brandImageContainer = brandImageContainer;
    }

    public async Task<BrandDto> GetAsync(Guid id)
    {
        var brand = await _repository.GetAsync(id);
        return ObjectMapper.Map<Brand, BrandDto>(brand);
    }

    public async Task<PagedResultDto<BrandDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var queryable = await _repository.GetQueryableAsync();
        var query = queryable
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Name" : input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);
        var brands = await AsyncExecuter.ToListAsync(query);
        var totalCount = await AsyncExecuter.CountAsync(queryable);
        return new PagedResultDto<BrandDto>(
            totalCount,
            ObjectMapper.Map<List<Brand>, List<BrandDto>>(brands)
        );
    }

    [Authorize(OfixPermissions.Brands.Create)]
    public async Task<BrandDto> CreateAsync(CreateUpdateBrandDto input)
    {
        var brand = ObjectMapper.Map<CreateUpdateBrandDto, Brand>(input);

       
        brand.Slug = GenerateSlug(input.Name);

        await _repository.InsertAsync(brand);
        return ObjectMapper.Map<Brand, BrandDto>(brand);
    }

    [Authorize(OfixPermissions.Brands.Edit)]
    public async Task<BrandDto> UpdateAsync(Guid id, CreateUpdateBrandDto input)
    {
        var brand = await _repository.GetAsync(id);
        ObjectMapper.Map(input, brand);
        brand.Slug = GenerateSlug(input.Name);

        
        await _repository.UpdateAsync(brand);
        return ObjectMapper.Map<Brand, BrandDto>(brand);
    }

    [Authorize(OfixPermissions.Brands.Edit)]
    public async Task UploadLogoBinaryAsync(Guid id, byte[] fileBytes, string fileName)
    {
        var brand = await _repository.GetAsync(id);

        if (fileBytes == null || fileBytes.Length == 0)
        {
            throw new AbpException("Dosya bos veya bulunamadi.");
        }

        var extension = Path.GetExtension(fileName);
        var blobname = $"{Guid.NewGuid():N}{extension}";

        using var stream = new MemoryStream(fileBytes);

        await _brandImageContainer.SaveAsync(blobname, stream);

        brand.LogoBlobName = blobname;
        brand.LogoFileName = fileName;

        await _repository.UpdateAsync(brand);
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent?> GetLogoAsync(Guid id)
    {
        var brand = await _repository.GetAsync(id);
        if (brand.LogoBlobName.IsNullOrWhiteSpace())
        {
            return null;
        }

        try
        {
            var stream = await _brandImageContainer.GetAsync(brand.LogoBlobName);
            var fileName = brand.LogoFileName ?? "logo.png";
            var contentType = GetContentType(fileName);

            return new RemoteStreamContent(stream, fileName, contentType);
        }
        catch
        {
            return null;
        }
    }



    [Authorize(OfixPermissions.Brands.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };
    }

    private static string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        text = text.Trim().ToLowerInvariant();

        text = text
              .Replace("ç", "c")
            .Replace("ğ", "g")
            .Replace("ı", "i")
            .Replace("ö", "o")
            .Replace("ş", "s")
            .Replace("ü", "u");

        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
        text = Regex.Replace(text, @"\s+", "-");
        text = Regex.Replace(text, @"-+", "-");

        return text.Trim('-');

    }
}
