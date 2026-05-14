using Ofix.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ofix.CarListings
{
    public class CreateUpdateCarListingDto
    {
        [Required]
        public Guid? BrandId { get; set; }

        [Required]
        public Guid? ModelId { get; set; }

        [Required]
        public Guid? SubModelId { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [Range(typeof(decimal), "1", "999999999")]
        public decimal Price { get; set; }

        [Range(CarListingConsts.MinYear, 2100)]
        public int Year { get; set; }

        [Range(CarListingConsts.MinMileage, CarListingConsts.MaxMileage)]
        public int Mileage { get; set; }

        [Required]
        public ListingStatus? ListingStatus { get; set; }

        [Required]
        public TransmissionType? Transmission { get; set; }

        [Required]
        public FuelType? FuelType { get; set; }

        [Required]
        public BodyShapeType? BodyShape { get; set; }

        [StringLength(10000)]
        public string Description { get; set; }
    }
}
