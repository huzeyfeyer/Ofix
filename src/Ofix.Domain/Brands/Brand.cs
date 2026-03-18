using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ofix.Brands
{
   public class Brand : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public bool Status { get; set; }
        public string Logo { get; set; }
        public string Slug { get; set; }

        protected Brand()
        {
        }

        public Brand(string name, int orderNo, bool status, string logo, string slug)
        {
            Name = name;
            OrderNo = orderNo;
            Status = status;
            Logo = logo;
            Slug = slug;
        }

        public Brand(Guid id, string name, int orderNo, bool status, string logo, string slug)
            : base(id)
        {
            Name = name;
            OrderNo = orderNo;
            Status = status;
            Logo = logo;
            Slug = slug;
        }
    }
}
