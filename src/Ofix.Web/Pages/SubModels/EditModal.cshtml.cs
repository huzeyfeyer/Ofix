using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ofix.Models;
using Ofix.SubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ofix.Web.Pages.SubModels
{
    public class EditModalModel : OfixPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateSubModelDto SubModel { get; set; }

        public List<SelectListItem> Models { get; set; } = new();

        private readonly ISubModelAppService _subModelAppService;
        private readonly IModelAppService _modelAppService;

        public EditModalModel(
            ISubModelAppService subModelAppService,
            IModelAppService modelAppService)
        {
            _subModelAppService = subModelAppService;
            _modelAppService = modelAppService;
        }

        public async Task OnGetAsync()
        {
            var subModelDto = await _subModelAppService.GetAsync(Id);
            SubModel = ObjectMapper.Map<SubModelDto, CreateUpdateSubModelDto>(subModelDto);

            var modelResult = await _modelAppService.GetListAsync(new ModelListInput
            {
                MaxResultCount = 1000
            });

            Models = modelResult.Items
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == SubModel.ModelId
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _subModelAppService.UpdateAsync(Id, SubModel);
            return NoContent();
        }
    }
}