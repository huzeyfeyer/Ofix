using Ofix.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ofix.CarListings
{
    public class CreateUpdateCarListingDto
    {
        public Guid SubModelId { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Year { get; set; }

        public int Mileage { get; set; }

        public ListingStatus ListingStatus { get; set; } = ListingStatus.Draft;

        public string? Description { get; set; }

        public TransmissionType Transmission { get; set; }

        public FuelType FuelType { get; set; }

        public BodyShapeType BodyShape { get; set; }
    }
}
