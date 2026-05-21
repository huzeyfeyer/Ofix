using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ofix.Brands;
using Ofix.CarListings;
using Ofix.Models;
using Ofix.Web.Pages.Index;
using Ofix.Web.Pages.Shared.Cards;
using Volo.Abp.Application.Dtos;

namespace Ofix.Web.Pages;

public class IndexModel : OfixPageModel
{
    private readonly ICarListingAppService _carListingAppService;
    private readonly IBrandAppService _brandAppService;
    private readonly IModelAppService _modelAppService;

    public HomePageViewModel ViewModel { get; set; } = new();

    public IndexModel(
        ICarListingAppService carListingAppService,
        IBrandAppService brandAppService,
        IModelAppService modelAppService)
    {
        _carListingAppService = carListingAppService;
        _brandAppService = brandAppService;
        _modelAppService = modelAppService;
    }

    public async Task OnGetAsync()
    {
        ViewModel.IsLoggedIn = CurrentUser.IsAuthenticated;
        ViewModel.MaxYear = DateTime.Now.Year + CarListingConsts.MaxYearOffsetFromCurrent;

        var brandResult = await _brandAppService.GetListAsync(new PagedAndSortedResultRequestDto
        {
            SkipCount = 0,
            MaxResultCount = 1000,
            Sorting = "orderNo, name"
        });

        var activeBrands = brandResult.Items
            .Where(b => b.IsActive)
            .OrderBy(b => b.OrderNo)
            .ThenBy(b => b.Name)
            .Select(b => new HomeBrandItemViewModel
            {
                Id = b.Id,
                Name = b.Name,
                LogoUrl = string.IsNullOrWhiteSpace(b.LogoBlobName)
                    ? string.Empty
                    : Url.Content($"~/api/app/brand/{b.Id}/logo") ?? string.Empty,
                Initials = GetInitials(b.Name)
            })
            .ToList();

        ViewModel.AllBrands = activeBrands;
        ViewModel.BrandStrip = activeBrands.Take(12).ToList();

        ViewModel.BodyShapes = Enum.GetValues<BodyShapeType>()
            .Select(shape => new HomeBodyShapeItemViewModel
            {
                Value = (int)shape,
                Label = L["Enum:BodyShapeType:" + shape].Value
            })
            .ToList();

        var listingResult = await _carListingAppService.GetPublishedListAsync(new CarListingListInput
        {
            SkipCount = 0,
            MaxResultCount = 8,
            Sorting = "creationTime DESC"
        });

        ViewModel.TotalListingCount = (int)listingResult.TotalCount;
        ViewModel.RecentListings = listingResult.Items.Select(MapToVehicleCard).ToList();
    }

    public async Task<JsonResult> OnGetModelsByBrandAsync(Guid brandId)
    {
        var modelResult = await _modelAppService.GetListAsync(new ModelListInput
        {
            BrandId = brandId,
            MaxResultCount = 1000,
            IsActive = true
        });

        var items = modelResult.Items
            .Select(x => new
            {
                id = x.Id,
                text = x.Name
            })
            .ToList();

        return new JsonResult(items);
    }

    public async Task<JsonResult> OnGetSearchResultCountAsync(
        Guid? brandId = null,
        Guid? modelId = null,
        int? minYear = null,
        decimal? maxPrice = null,
        int? bodyShape = null)
    {
        BodyShapeType? bodyShapeFilter = null;
        if (bodyShape.HasValue && Enum.IsDefined(typeof(BodyShapeType), bodyShape.Value))
        {
            bodyShapeFilter = (BodyShapeType)bodyShape.Value;
        }

        var result = await _carListingAppService.GetPublishedListAsync(new CarListingListInput
        {
            SkipCount = 0,
            MaxResultCount = 1,
            BrandId = brandId,
            ModelId = modelId,
            MinYear = minYear,
            MaxPrice = maxPrice,
            BodyShape = bodyShapeFilter
        });

        return new JsonResult(new { count = result.TotalCount });
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

        if (blobOrUrl.StartsWith('/'))
        {
            return blobOrUrl;
        }

        return "/uploads/car-listings/" + blobOrUrl.TrimStart('/');
    }

    private static string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "?";
        }

        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            return $"{parts[0][0]}{parts[1][0]}".ToUpperInvariant();
        }

        return name.Length >= 2 ? name[..2].ToUpperInvariant() : name.ToUpperInvariant();
    }
}
