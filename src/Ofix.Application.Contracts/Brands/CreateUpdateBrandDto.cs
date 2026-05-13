using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ofix.Brands
{
    public class CreateUpdateBrandDto
    {
        [Required]
        [StringLength(BrandsConsts.MaxNameLength)]
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
        public bool IsActive { get; set; } = true;
       
        [StringLength(256)]
        public string Slug { get; set; } = string.Empty;
    }
}
