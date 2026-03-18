using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ofix.Models
{
    public class ModelAppService :
        CrudAppService<
            Model,
            ModelDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateModelDto>,
        IModelAppService
    {
        public ModelAppService(IRepository<Model, Guid> repository)
            : base(repository)
        {
        }
    }
}