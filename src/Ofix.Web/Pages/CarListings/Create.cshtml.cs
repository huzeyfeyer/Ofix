using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ofix.Brands;
using Ofix.CarListings;
using Ofix.Models;
using Ofix.SubModels;
using Volo.Abp.Application.Dtos;

namespace Ofix.Web.Pages.CarListings
{
    public class CreateModel : OfixPageModel
    {
        [BindProperty]
        public CreateUpdateCarListingDto CarListing { get; set; }

        public List<SelectListItem> Brands { get; set; } = new();
        public List<SelectListItem> Models { get; set; } = new();
        public List<SelectListItem> SubModels { get; set; } = new();
        public List<SelectListItem> Years { get; set; } = new();

        public List<SelectListItem> ListingStatuses { get; set; } = new();
        public List<SelectListItem> Transmissions { get; set; } = new();
        public List<SelectListItem> FuelTypes { get; set; } = new();
        public List<SelectListItem> BodyShapes { get; set; } = new();

        public string CurrencySymbol => GetCurrencySymbol();

        private string GetCurrencySymbol()
        {
            var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            return language switch
            {
                "tr" => "₺",
                "nl" => "€"

            };
        }


        private readonly ICarListingAppService _carListingAppService;
        private readonly IBrandAppService _brandAppService;
        private readonly IModelAppService _modelAppService;
        private readonly ISubModelAppService _subModelAppService;

        public CreateModel(
            ICarListingAppService carListingAppService,
            IBrandAppService brandAppService,
            IModelAppService modelAppService,
            ISubModelAppService subModelAppService)
        {
            _carListingAppService = carListingAppService;
            _brandAppService = brandAppService;
            _modelAppService = modelAppService;
            _subModelAppService = subModelAppService;
        }

        public async Task OnGetAsync()
        {
            CarListing = new CreateUpdateCarListingDto();
            await LoadLookupsAsync();
        }

        public async Task<JsonResult> OnGetModelsByBrandAsync(Guid brandId)
        {
            var modelResult = await _modelAppService.GetListAsync(new ModelListInput
            {
                BrandId = brandId,
                MaxResultCount = 1000
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

        public async Task<JsonResult> OnGetSubModelsByModelAsync(Guid modelId)
        {
            var subModelResult = await _subModelAppService.GetListAsync(new SubModelListInput
            {
                ModelId = modelId,
                MaxResultCount = 1000
            });

            var items = subModelResult.Items
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToList();

            return new JsonResult(items);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync();
                return Page();
            }

            await _carListingAppService.CreateAsync(CarListing);
            return RedirectToPage("/CarListings/Index");
        }

        private async Task LoadLookupsAsync()
        {
            var brandResult = await _brandAppService.GetListAsync(new PagedAndSortedResultRequestDto
            {
                MaxResultCount = 1000
            });

            Brands = new List<SelectListItem>
            {
                new SelectListItem { Text = L["SelectBrand"].Value, Value = "" }
            };

            Brands.AddRange(
                brandResult.Items.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            );

            Models = new List<SelectListItem>
            {
                new SelectListItem { Text = L["SelectModel"].Value, Value = "" }
            };

            SubModels = new List<SelectListItem>
            {
                new SelectListItem { Text = L["SelectSubModel"].Value, Value = "" }
            };

            Years = BuildYearList();

            ListingStatuses = BuildEnumSelectList<ListingStatus>(L["SelectListingStatus"].Value);
            Transmissions = BuildEnumSelectList<TransmissionType>(L["SelectTransmission"].Value);
            FuelTypes = BuildEnumSelectList<FuelType>(L["SelectFuelType"].Value);
            BodyShapes = BuildEnumSelectList<BodyShapeType>(L["SelectBodyShape"].Value);

            if (CarListing?.BrandId != null && CarListing.BrandId != Guid.Empty)
            {
                var modelResult = await _modelAppService.GetListAsync(new ModelListInput
                {
                    BrandId = CarListing.BrandId.Value,
                    MaxResultCount = 1000
                });

                Models.AddRange(
                    modelResult.Items.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    })
                );
            }

            if (CarListing?.ModelId != null && CarListing.ModelId != Guid.Empty)
            {
                var subModelResult = await _subModelAppService.GetListAsync(new SubModelListInput
                {
                    ModelId = CarListing.ModelId.Value,
                    MaxResultCount = 1000
                });

                SubModels.AddRange(
                    subModelResult.Items.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    })
                );
            }
        }

        private List<SelectListItem> BuildYearList()
        {
            var maxYear = DateTime.Now.Year + CarListingConsts.MaxYearOffsetFromCurrent;

            var years = new List<SelectListItem>
            {
                new SelectListItem { Text = L["SelectYear"].Value, Value = "" }
            };

            years.AddRange(
                Enumerable.Range(CarListingConsts.MinYear, maxYear - CarListingConsts.MinYear + 1)
                    .Reverse()
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString()
                    })
            );

            return years;
        }

        private List<SelectListItem> BuildEnumSelectList<TEnum>(string placeholder) where TEnum : Enum
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = placeholder,
                    Value = ""
                }
            };

            items.AddRange(
                Enum.GetValues(typeof(TEnum))
                    .Cast<TEnum>()
                    .Select(x => new SelectListItem
                    {
                        Text = L[$"Enum:{typeof(TEnum).Name}:{x}"].Value,
                        Value = Convert.ToInt32(x).ToString()
                    })
            );

            return items;
        }
    }
}