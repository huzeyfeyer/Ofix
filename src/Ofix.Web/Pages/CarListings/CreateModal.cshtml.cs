using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ofix.CarListings;
using Ofix.SubModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofix.Web.Pages.CarListings
{
    public class CreateModalModel : OfixPageModel
    {
        [BindProperty]
        public CreateUpdateCarListingDto CarListing { get; set; }

        public List<SelectListItem> SubModels { get; set; } = new();

        private readonly ICarListingAppService _carListingAppService;
        private readonly ISubModelAppService _subModelAppService;

        public CreateModalModel(
            ICarListingAppService carListingAppService,
            ISubModelAppService subModelAppService)
        {
            _carListingAppService = carListingAppService;
            _subModelAppService = subModelAppService;
        }

        public async Task OnGetAsync()
        {
            CarListing = new CreateUpdateCarListingDto();

            var subModelResult = await _subModelAppService.GetListAsync(new SubModelListInput
            {
                MaxResultCount = 1000
            });

            SubModels = subModelResult.Items
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _carListingAppService.CreateAsync(CarListing);
            return new NoContentResult();
        }
    }
}