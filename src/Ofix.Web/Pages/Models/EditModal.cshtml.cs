using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ofix.Brands;
using Ofix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ofix.Web.Pages.Models
{
    public class EditModalModel : OfixPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateModelDto Model { get; set; }

        public List<SelectListItem> Brands { get; set; } = new();

        private readonly IModelAppService _modelAppService;
        private readonly IBrandAppService _brandAppService;

        public EditModalModel(
            IModelAppService modelAppService,
            IBrandAppService brandAppService)
        {
            _modelAppService = modelAppService;
            _brandAppService = brandAppService;
        }

        public async Task OnGetAsync()
        {
            var modelDto = await _modelAppService.GetAsync(Id);
            Model = ObjectMapper.Map<ModelDto, CreateUpdateModelDto>(modelDto);

            var brandResult = await _brandAppService.GetListAsync(new PagedAndSortedResultRequestDto
            {
                MaxResultCount = 1000
            });

            Brands = brandResult.Items
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == Model.BrandId
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _modelAppService.UpdateAsync(Id, Model);
            return NoContent();
        }
    }
}