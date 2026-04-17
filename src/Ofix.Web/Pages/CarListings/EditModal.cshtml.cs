using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ofix.CarListings;
using Ofix.SubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofix.Web.Pages.CarListings
{
    public class EditModalModel : OfixPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateCarListingDto CarListing { get; set; }

        public List<SelectListItem> SubModels { get; set; } = new();

        private readonly ICarListingAppService _carListingAppService;
        private readonly ISubModelAppService _subModelAppService;

        public EditModalModel(
            ICarListingAppService carListingAppService,
            ISubModelAppService subModelAppService)
        {
            _carListingAppService = carListingAppService;
            _subModelAppService = subModelAppService;
        }

        public async Task OnGetAsync()
        {
            var carListingDto = await _carListingAppService.GetAsync(Id);
            CarListing = ObjectMapper.Map<CarListingDto, CreateUpdateCarListingDto>(carListingDto);

            var subModelResult = await _subModelAppService.GetListAsync(new SubModelListInput
            {
                MaxResultCount = 1000
            });

            SubModels = subModelResult.Items
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == CarListing.SubModelId
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _carListingAppService.UpdateAsync(Id, CarListing);
            return new NoContentResult();
        }
    }
}