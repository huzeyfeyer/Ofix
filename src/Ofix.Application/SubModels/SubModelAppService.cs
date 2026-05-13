using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ofix.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace Ofix.SubModels
{
    [Authorize(OfixPermissions.SubModels.Default)]
    public class SubModelAppService : ApplicationService, ISubModelAppService
    {
        private readonly IRepository<SubModel, Guid> _repository;

        public SubModelAppService(IRepository<SubModel, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<SubModelDto> GetAsync(Guid id)
        {
            var subModel = await _repository.GetAsync(id);
            return ObjectMapper.Map<SubModel, SubModelDto>(subModel);
        }

        public async Task<PagedResultDto<SubModelDto>> GetListAsync(SubModelListInput input)
        {
            var queryable = await _repository.GetQueryableAsync();

            var filteredQuery = queryable
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name!))
                .WhereIf(input.ModelId.HasValue, x => x.ModelId == input.ModelId.Value)
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);

            var subModels = await AsyncExecuter.ToListAsync(
                filteredQuery
                    .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Name" : input.Sorting)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var totalCount = await AsyncExecuter.CountAsync(filteredQuery);

            return new PagedResultDto<SubModelDto>(
                totalCount,
                ObjectMapper.Map<List<SubModel>, List<SubModelDto>>(subModels)
            );
        }

        [Authorize(OfixPermissions.SubModels.Create)]
        public async Task<SubModelDto> CreateAsync(CreateUpdateSubModelDto input)
        {
            var subModel = ObjectMapper.Map<CreateUpdateSubModelDto, SubModel>(input);

            subModel.Slug = GenerateSlug(input.Name);

            await _repository.InsertAsync(subModel, autoSave: true);

            return ObjectMapper.Map<SubModel, SubModelDto>(subModel);
        }

        [Authorize(OfixPermissions.SubModels.Edit)]
        public async Task<SubModelDto> UpdateAsync(Guid id, CreateUpdateSubModelDto input)
        {
            var subModel = await _repository.GetAsync(id);

            subModel.ModelId = input.ModelId;
            subModel.Name = input.Name;
            subModel.OrderNo = input.OrderNo;
            subModel.IsActive = input.IsActive;
            subModel.Slug = GenerateSlug(input.Name);

            await _repository.UpdateAsync(subModel, autoSave: true);

            return ObjectMapper.Map<SubModel, SubModelDto>(subModel);
        }

        [Authorize(OfixPermissions.SubModels.Delete)]
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
}