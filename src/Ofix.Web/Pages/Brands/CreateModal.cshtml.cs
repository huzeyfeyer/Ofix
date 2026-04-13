using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ofix.Brands;
using System.Threading.Tasks;


namespace Ofix.Web.Pages.Brands
{
    public class CreateModalModel : OfixPageModel
    {
        [BindProperty]
        public CreateUpdateBrandDto Brand { get; set; }

        private readonly IBrandAppService _brandAppService;

        public CreateModalModel(IBrandAppService brandAppService)
        {
            _brandAppService = brandAppService;
        }

        public void OnGet()
        {
            Brand = new CreateUpdateBrandDto();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _brandAppService.CreateAsync(Brand);
            return new JsonResult(result);
        }

    }
}
