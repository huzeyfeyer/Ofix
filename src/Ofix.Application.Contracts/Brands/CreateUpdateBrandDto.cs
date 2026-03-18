using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ofix.Brands
{
    public class CreateUpdateBrandDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
        public bool Status { get; set; }
        [StringLength(512)]
        public string Logo { get; set; } = string.Empty;
        [StringLength(256)]
        public string Slug { get; set; } = string.Empty;
    }
}
