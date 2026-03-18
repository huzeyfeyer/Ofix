using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ofix.SubModels
{
    public class SubModel : FullAuditedAggregateRoot<Guid>
    {
        public Guid ModelId { get; set; }
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public bool Status { get; set; }
        public string? Slug { get; set; }

        protected SubModel()
        {
        }

        public SubModel(
            Guid modelId,
            string name,
            int orderNo,
            bool status,
            string? slug = null
        )
        {
            ModelId = modelId;
            Name = name;
            OrderNo = orderNo;
            Status = status;
            Slug = slug;
        }

        public SubModel(
            Guid id,
            Guid modelId,
            string name,
            int orderNo,
            bool status,
            string? slug = null
        ) : base(id)
        {
            ModelId = modelId;
            Name = name;
            OrderNo = orderNo;
            Status = status;
            Slug = slug;
        }
    }
}
