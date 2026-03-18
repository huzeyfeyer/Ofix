using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ofix.SubModels
{
    public class SubModelAppService :
        CrudAppService<
            SubModel,
            SubModelDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateSubModelDto>,
        ISubModelAppService
    {
        public SubModelAppService(IRepository<SubModel, Guid> repository)
            : base(repository)
        {
        }
    }
}