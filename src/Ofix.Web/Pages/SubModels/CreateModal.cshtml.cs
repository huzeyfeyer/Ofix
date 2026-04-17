using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ofix.Models;
using Ofix.SubModels;
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

        public List<SelectListItem> Models { get; set; } = new();

        private readonly ISubModelAppService _subModelAppService;
        private readonly IModelAppService _modelAppService;

        public CreateModalModel(
            ISubModelAppService subModelAppService,
            IModelAppService modelAppService)
        {
            _subModelAppService = subModelAppService;
            _modelAppService = modelAppService;
        }

        public async Task OnGetAsync()
        {
            SubModel = new CreateUpdateSubModelDto();

            var modelResult = await _modelAppService.GetListAsync(new ModelListInput
            {
                MaxResultCount = 1000
            });

            Models = modelResult.Items
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _subModelAppService.CreateAsync(SubModel);
            return NoContent();
        }
    }
}