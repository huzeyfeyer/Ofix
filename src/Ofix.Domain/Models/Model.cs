using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ofix.Models
{
   public class Model : FullAuditedAggregateRoot<Guid>
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
        public ListingStatus ListingStatus { get; set; } = Models.ListingStatus.Draft;
        public string Slug { get; set; } = string.Empty;

       
    }
}
