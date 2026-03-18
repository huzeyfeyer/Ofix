using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.Features
{
    public class FeatureDto : FullAuditedEntityDto<Guid>
    {
        public Guid FeatureCategoryId { get; set; }
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public bool Status { get; set; }
    }
}
