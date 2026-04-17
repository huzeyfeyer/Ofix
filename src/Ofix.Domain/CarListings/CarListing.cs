using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ofix.CarListings
{
    public class CarListing : FullAuditedAggregateRoot<Guid>
    {
        public Guid SubModelId { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Year { get; set; }

        public int Mileage { get; set; }

        public Models.ListingStatus ListingStatus { get; set; }

        public string? Description { get; set; }

        public TransmissionType Transmission { get; set; }

        public FuelType FuelType { get; set; }

        public BodyShapeType BodyShape { get; set; }
    }
}
