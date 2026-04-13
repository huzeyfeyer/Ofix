using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ofix.SubModels
{
    public class SubModel : FullAuditedAggregateRoot<Guid>
    {
        public Guid ModelId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int OrderNo { get; set; }

        public Ofix.Models.ListingStatus ListingStatus { get; set; } = Ofix.Models.ListingStatus.Draft;

        public string Slug { get; set; } = string.Empty;


    }
}
