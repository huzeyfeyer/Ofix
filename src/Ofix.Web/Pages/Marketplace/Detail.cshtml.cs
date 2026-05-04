using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ofix.CarListings;
using Volo.Abp.Domain.Entities;

namespace Ofix.Web.Pages.Marketplace;

[AllowAnonymous]
public class DetailModel : OfixPageModel
{
    private readonly ICarListingAppService _carListingAppService;

    public DetailModel(ICarListingAppService carListingAppService)
    {
        _carListingAppService = carListingAppService;
    }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public CarListingDto Listing { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Listing = await _carListingAppService.GetPublishedDetailAsync(Id);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }

        return Page();
    }

    public string ToonAfbeeldingUrl(string? blobOfUrl)
    {
        if (string.IsNullOrWhiteSpace(blobOfUrl))
        {
            return "https://placehold.co/960x540?text=Ofix";
        }

        if (blobOfUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            blobOfUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return blobOfUrl;
        }

        if (blobOfUrl.StartsWith('/'))
        {
            return blobOfUrl;
        }

        return "/uploads/car-listings/" + blobOfUrl.TrimStart('/');
    }
}
