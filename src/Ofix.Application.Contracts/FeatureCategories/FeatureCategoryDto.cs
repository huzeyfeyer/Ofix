using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.FeatureCategories
{
    public class FeatureCategoryDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string? Icon { get; set; }
        public int OrderNo { get; set; }
        public bool Status { get; set; }
    }
}
