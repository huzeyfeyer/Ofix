using Ofix.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.CarListings
{
    public class CarListingListInput : PagedAndSortedResultRequestDto
    {
        public Guid? BrandId { get; set; }

        public Guid? ModelId { get; set; }

        public Guid? SubModelId { get; set; }

        public string? Title { get; set; }

        public ListingStatus? ListingStatus { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? MinMileage { get; set; }

        public int? MaxMileage { get; set; }

        public int? MinYear { get; set; }

        public int? MaxYear { get; set; }

        public FuelType? FuelType { get; set; }

        public TransmissionType? Transmission { get; set; }

        public BodyShapeType? BodyShape { get; set; }
    }
}
