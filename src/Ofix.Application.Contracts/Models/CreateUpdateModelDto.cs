using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Ofix.Models
{
        public class CreateUpdateModelDto
        {
            public Guid BrandId { get; set; }

            [Required]
            [StringLength(ModelConsts.MaxNameLength)]
            public string Name { get; set; } = string.Empty;

            public int OrderNo { get; set; }

            public ListingStatus ListingStatus { get; set; } = Models.ListingStatus.Draft;

        [StringLength(ModelConsts.MaxNameLength)]
            public string Slug { get; set; } = string.Empty;
        }
    
}