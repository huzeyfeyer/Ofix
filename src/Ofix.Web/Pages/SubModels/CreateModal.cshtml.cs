using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ofix.Brands;
using Ofix.Models;
using Ofix.SubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;


namespace Ofix.Web.Pages.SubModels
{
    public class CreateModalModel : OfixPageModel
    {
        [BindProperty]
        public CreateUpdateSubModelDto SubModel { get; set; }

        [BindProperty]
        public Guid? BrandId { get; set; }

        public List<SelectListItem> Brands { get; set; } = new();
        public List<SelectListItem> Models { get; set; } = new();

        private readonly ISubModelAppService _subModelAppService;
        private readonly IModelAppService _modelAppService;
        private readonly IBrandAppService _brandAppService;

        public CreateModalModel(
            ISubModelAppService subModelAppService,
            IModelAppService modelAppService, IBrandAppService brandAppService)
        {
            _subModelAppService = subModelAppService;
            _modelAppService = modelAppService;
            _brandAppService = brandAppService;
        }

        public async Task OnGetAsync()
        {
            SubModel = new CreateUpdateSubModelDto();

            var brandResult = await _brandAppService.GetListAsync(new PagedAndSortedResultRequestDto
            {
                MaxResultCount = 1000
            });

            Brands = brandResult.Items
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
                .ToList();

            Models = new List<SelectListItem>();
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

        public async Task<IActionResult> OnPostAsync()
        {
            await _subModelAppService.CreateAsync(SubModel);
            return NoContent();
        }
    }
}