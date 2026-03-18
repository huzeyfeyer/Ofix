using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ofix.Features
{
    public class FeatureAppService :
      CrudAppService<
          Feature,
          FeatureDto,
          Guid,
          PagedAndSortedResultRequestDto,
          CreateUpdateFeatureDto>,
      IFeatureAppService
    {
        public FeatureAppService(IRepository<Feature, Guid> repository)
            : base(repository)
        {
        }
    }
}
