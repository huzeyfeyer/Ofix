using Ofix.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.CarListings
{
    public class CarListingListInput : PagedAndSortedResultRequestDto
    {
        public Guid? SubModelId { get; set; }

        public string? Title { get; set; }

        public ListingStatus? ListingStatus { get; set; }
    }
}
