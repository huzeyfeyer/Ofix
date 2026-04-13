using Ofix.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.SubModels
{
    public class SubModelListInput : PagedAndSortedResultRequestDto
    {
        public Guid? ModelId { get; set; }

        public string? Name { get; set; }

        public ListingStatus? ListingStatus { get; set; }
    }
}
