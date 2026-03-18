using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ofix.FeatureCategories
{
    public class FeatureCategory : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string? Icon { get; set; }
        public int OrderNo { get; set; }
        public bool Status { get; set; }

        public FeatureCategory()
        {
        }

        public FeatureCategory(
            string name,
            string? icon,
            int orderNo,
            bool status
        )
        {
            Name = name;
            Icon = icon;
            OrderNo = orderNo;
            Status = status;
        }

        public FeatureCategory(
            Guid id,
            string name,
            string? icon,
            int orderNo,
            bool status
        ) : base(id)
        {
            Name = name;
            Icon = icon;
            OrderNo = orderNo;
            Status = status;
        }
    }
}
