using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ofix.FeatureCategories
{
   public interface IFeatureCategoryAppService :
        ICrudAppService<
            FeatureCategoryDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateFeatureCategoryDto>
    {
    }
}
