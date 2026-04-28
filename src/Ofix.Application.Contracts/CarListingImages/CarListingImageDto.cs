using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.CarListingImages
{
    public class CarListingImageDto : AuditedEntityDto<Guid>
    {
        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string Url { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public bool IsCover { get; set; }
    }
}
