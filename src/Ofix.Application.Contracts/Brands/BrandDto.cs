using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.Brands
{
    public class BrandDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public bool Status { get; set; }
        public string Logo { get; set; }
        public string Slug { get; set; }
    }
}
