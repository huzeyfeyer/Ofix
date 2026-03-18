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
        public bool Status { get; set; }
        public string Slug { get; set; } = string.Empty;

        protected Model()
        {
        }

        public Model(Guid brandId, string name, int orderNo, bool status, string slug)
        {
            BrandId = brandId;
            Name = name;
            OrderNo = orderNo;
            Status = status;
            Slug = slug;
        }

        public Model(Guid id, Guid brandId, string name, int orderNo, bool status, string slug)
            : base(id)
        {
            BrandId = brandId;
            Name = name;
            OrderNo = orderNo;
            Status = status;
            Slug = slug;
        }
    }
}
