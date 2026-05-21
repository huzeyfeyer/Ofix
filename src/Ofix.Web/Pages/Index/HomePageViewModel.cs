using System;
using System.Collections.Generic;
using Ofix.Web.Pages.Shared.Cards;

namespace Ofix.Web.Pages.Index;

public class HomePageViewModel
{
    public List<HomeBrandItemViewModel> BrandStrip { get; set; } = new();

    public List<HomeBrandItemViewModel> AllBrands { get; set; } = new();

    public List<HomeBodyShapeItemViewModel> BodyShapes { get; set; } = new();

    public List<VehicleCardViewModel> RecentListings { get; set; } = new();

    public int TotalListingCount { get; set; }

    public bool IsLoggedIn { get; set; }

    public int MaxYear { get; set; }
}

public class HomeBrandItemViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;

    public string Initials { get; set; } = string.Empty;
}

public class HomeBodyShapeItemViewModel
{
    public int Value { get; set; }

    public string Label { get; set; } = string.Empty;
}
