using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ofix.Features
{
    public interface IFeatureAppService :
     ICrudAppService<
         FeatureDto,
         Guid,
         PagedAndSortedResultRequestDto,
         CreateUpdateFeatureDto>
    {
    }
}
