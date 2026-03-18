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
            [StringLength(128)]
            public string Name { get; set; } = string.Empty;

            public int OrderNo { get; set; }

            public bool Status { get; set; }

            [StringLength(256)]
            public string Slug { get; set; } = string.Empty;
        }
    
}