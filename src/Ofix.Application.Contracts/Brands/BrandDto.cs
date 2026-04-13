using Ofix.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.Brands
{
    public class BrandDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
        public ListingStatus Status { get; set; } = ListingStatus.Draft;

        public string? LogoBlobName { get; set; }
        public string? LogoFileName { get; set; }
        public string Slug { get; set; } = string.Empty;
    }
}
