using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ofix.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using Ofix.Models;
using Volo.Abp;
using Volo.Abp.Linq;

namespace Ofix.Models;

    [Authorize(OfixPermissions.Models.Default)]
    public class ModelAppService : ApplicationService, IModelAppService
    {

     private readonly IRepository<Model, Guid> _repository;

    public ModelAppService(IRepository<Model, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<ModelDto> GetAsync(Guid id)
    {
        var model = await _repository.GetAsync(id);
        return ObjectMapper.Map<Model, ModelDto>(model);
    }

    public async Task<PagedResultDto<ModelDto>> GetListAsync(ModelListInput input)
    {
        var queryable = await _repository.GetQueryableAsync();

        var query = queryable
             .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name!))
                .WhereIf(input.BrandId.HasValue, x => x.BrandId == input.BrandId.Value)
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value)
                .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Name" : input.Sorting)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

        var models = await AsyncExecuter.ToListAsync(query);

        var totalCount = await AsyncExecuter.CountAsync(
              queryable
                  .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name!))
                  .WhereIf(input.BrandId.HasValue, x => x.BrandId == input.BrandId.Value)
                  .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value)
          );

        return new PagedResultDto<ModelDto>(
            totalCount,
            ObjectMapper.Map<List<Model>, List<ModelDto>>(models)
        );
    }


    [Authorize(OfixPermissions.Models.Create)]
    public async Task<ModelDto> CreateAsync(CreateUpdateModelDto input)
    {
        var model = ObjectMapper.Map<CreateUpdateModelDto, Model>(input);

        model.Slug = GenerateSlug(input.Name);

        await _repository.InsertAsync(model);
        return ObjectMapper.Map<Model, ModelDto>(model);
    }

    [Authorize(OfixPermissions.Models.Edit)]
    public async Task<ModelDto> UpdateAsync(Guid id, CreateUpdateModelDto input)
    {
        var model = await _repository.GetAsync(id);

        model.Name = input.Name;
        model.OrderNo = input.OrderNo;
        model.IsActive = input.IsActive;
        model.BrandId = input.BrandId;
        model.Slug = GenerateSlug(input.Name);

        await _repository.UpdateAsync(model, autoSave: true);

        return ObjectMapper.Map<Model, ModelDto>(model);
    }

    [Authorize(OfixPermissions.Models.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
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