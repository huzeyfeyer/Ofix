using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ofix.FeatureCategories
{
    public class FeatureCategoryAppService :
     CrudAppService<
         FeatureCategory,
         FeatureCategoryDto,
         Guid,
         PagedAndSortedResultRequestDto,
         CreateUpdateFeatureCategoryDto>,
     IFeatureCategoryAppService
    {
        public FeatureCategoryAppService(IRepository<FeatureCategory, Guid> repository)
            : base(repository)
        {
        }
    }
}
