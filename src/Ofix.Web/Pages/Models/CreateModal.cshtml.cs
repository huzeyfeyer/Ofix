using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ofix.Brands;
using Ofix.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ofix.Web.Pages.Models
{
    public class CreateModalModel : OfixPageModel
    {
        [BindProperty]
        public CreateUpdateModelDto Model { get; set; }

        public List<SelectListItem> Brands { get; set; } = new();

        private readonly IModelAppService _modelAppService;
        private readonly IBrandAppService _brandAppService;

        public CreateModalModel(
            IModelAppService modelAppService,
            IBrandAppService brandAppService)
        {
            _modelAppService = modelAppService;
            _brandAppService = brandAppService;
        }

        public async Task OnGetAsync()
        {
            Model = new CreateUpdateModelDto();

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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _modelAppService.CreateAsync(Model);
            return NoContent();
        }
    }
}