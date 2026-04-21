using Ofix.Models;
using Ofix.SubModels;
using Ofix.Brands;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ofix.CarListings
{
    public class CarListingDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public Guid? BrandId { get; set; }
        public string BrandName { get; set; }
        public Guid? ModelId { get; set; }
        public string ModelName { get; set; }
        public Guid? SubModelId { get; set; }
        public string SubModelName { get; set; }

        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public ListingStatus ListingStatus { get; set; }
        public TransmissionType Transmission { get; set; }
        public FuelType FuelType { get; set; }
        public BodyShapeType BodyShape { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
