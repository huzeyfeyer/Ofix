using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ofix.CarListings;
using Ofix.Web.Pages.Shared.Cards;

namespace Ofix.Web.Pages.Marketplace;

public class IndexModel : OfixPageModel
{
    private readonly ICarListingAppService _carListingAppService;

    public IndexModel(ICarListingAppService carListingAppService)
    {
        _carListingAppService = carListingAppService;
    }

    public async Task<PartialViewResult> OnGetKaartenAsync(
        int skipCount = 0,
        int maxResultCount = 9,
        string? title = null,
        Guid? brandId = null,
        Guid? modelId = null,
        Guid? subModelId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? minMileage = null,
        int? maxMileage = null,
        int? minYear = null,
        int? maxYear = null,
        FuelType? fuelType = null,
        TransmissionType? transmission = null,
        BodyShapeType? bodyShape = null,
        string? sorting = null)
    {
        maxResultCount = Math.Clamp(maxResultCount, 1, 50);

        var input = new CarListingListInput
        {
            SkipCount = skipCount,
            MaxResultCount = maxResultCount,
            Title = string.IsNullOrWhiteSpace(title) ? null : title.Trim(),
            BrandId = brandId,
            ModelId = modelId,
            SubModelId = subModelId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            MinMileage = minMileage,
            MaxMileage = maxMileage,
            MinYear = minYear,
            MaxYear = maxYear,
            FuelType = fuelType,
            Transmission = transmission,
            BodyShape = bodyShape,
            Sorting = string.IsNullOrWhiteSpace(sorting) ? "creationTime DESC" : sorting
        };

        var result = await _carListingAppService.GetPublishedListAsync(input);

        Response.Headers["X-Total-Count"] = result.TotalCount.ToString(CultureInfo.InvariantCulture);

        var cards = result.Items.Select(MapToVehicleCard).ToList();

        return Partial("_MarketplaceVehicleCards", cards);
    }

    private VehicleCardViewModel MapToVehicleCard(CarListingDto dto)
    {
        return new VehicleCardViewModel
        {
            Id = dto.Id.ToString(),
            DetailUrl = Url.Page("/Marketplace/Detail", values: new { id = dto.Id }) ?? "#",
            ImageUrl = ResolveCoverImageUrl(dto.CoverImageUrl),
            Title = dto.Title ?? string.Empty,
            PriceText = "€ " + dto.Price.ToString("N0", CultureInfo.CurrentUICulture),
            Year = dto.Year,
            Mileage = dto.Mileage,
            FuelTypeText = L["Enum:FuelType:" + dto.FuelType].Value,
            IsNew = false
        };
    }

    private static string ResolveCoverImageUrl(string? blobOrUrl)
    {
        if (string.IsNullOrWhiteSpace(blobOrUrl))
        {
            return "https://placehold.co/640x360?text=Ofix";
        }

        if (blobOrUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            blobOrUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return blobOrUrl;
        }

        // BlobName uit de app is al bv. /uploads/car-listings/{bestandsnaam}
        if (blobOrUrl.StartsWith('/'))
        {
            return blobOrUrl;
        }

        return "/uploads/car-listings/" + blobOrUrl.TrimStart('/');
    }
}
