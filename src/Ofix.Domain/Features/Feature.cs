using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ofix.Features
{
    public class Feature : FullAuditedAggregateRoot<Guid>
    {
        public Guid FeatureCategoryId { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public bool Status { get; set; }

        public Feature()
        {
        }

        public Feature(
            Guid featureCategoryId,
            string name,
            int orderNo,
            bool status
        )
        {
            FeatureCategoryId = featureCategoryId;
            Name = name;
            OrderNo = orderNo;
            Status = status;
        }

        public Feature(
            Guid id,
            Guid featureCategoryId,
            string name,
            int orderNo,
            bool status
        ) : base(id)
        {
            FeatureCategoryId = featureCategoryId;
            Name = name;
            OrderNo = orderNo;
            Status = status;
        }
    }
}
