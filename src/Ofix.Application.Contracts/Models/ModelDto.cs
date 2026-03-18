using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.Models
{
    public class ModelDto : AuditedEntityDto<Guid>
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
        public bool Status { get; set; }
        public string Slug { get; set; } = string.Empty;
    }
}
