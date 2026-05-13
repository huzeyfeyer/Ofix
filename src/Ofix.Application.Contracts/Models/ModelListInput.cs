using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.Models
{
    public class ModelListInput : PagedAndSortedResultRequestDto
    {
        public Guid? BrandId { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
