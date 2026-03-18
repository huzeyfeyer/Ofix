using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ofix.SubModels
{
    public interface ISubModelAppService :
        ICrudAppService<
            SubModelDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateSubModelDto>
    {
    }
}