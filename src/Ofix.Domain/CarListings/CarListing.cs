using Ofix.Brands;
using Ofix.Models;
using Ofix.SubModels;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Ofix.CarListingImages;

namespace Ofix.CarListings
{
    public class CarListing : FullAuditedAggregateRoot<Guid>
    {
        public Guid? BrandId { get; set; }
        public Guid? ModelId { get; set; }
        public Guid SubModelId { get; set; }

        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public ListingStatus ListingStatus { get; set; }
        public string? Description { get; set; }
        public TransmissionType Transmission { get; set; }
        public FuelType FuelType { get; set; }
        public BodyShapeType BodyShape { get; set; }

        public Brand? Brand { get; set; }
        public Model? Model { get; set; }
        public SubModel? SubModel { get; set; }

        public ICollection<CarListingImage> Images { get; set; } = new List<CarListingImage>();
    }
}
