using Ofix.CarListings;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ofix.CarListingImages
{
    public class CarListingImage : FullAuditedAggregateRoot<Guid>
    {
        public Guid CarListingId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string BlobName { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public bool IsCover { get; set; }

        public CarListing? CarListing { get; set; }
        public ICollection<CarListingImage> Images { get; set; } = new List<CarListingImage>();

    }
}
