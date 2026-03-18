using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ofix.SubModels
{
        public class CreateUpdateSubModelDto
        {
            public Guid ModelId { get; set; }

            [Required]
            [StringLength(128)]
            public string Name { get; set; } = string.Empty;

            public int OrderNo { get; set; }

            public bool Status { get; set; }

            [StringLength(256)]
            public string Slug { get; set; } = string.Empty;
        }
    
}