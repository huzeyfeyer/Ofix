using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ofix.Brands;
using Ofix.Permissions;

namespace Ofix.Controllers;

[Route("api/app/brand")]
[Authorize(OfixPermissions.Brands.Edit)]
public class BrandLogoController : OfixController
{
    private readonly IBrandAppService _brandAppService;

    public BrandLogoController(IBrandAppService brandAppService)
    {
        _brandAppService = brandAppService;
    }

    [HttpPost("{id}/upload-logo")]
    public async Task<IActionResult> UploadLogoAsync(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Dosya bos veya bulunamadi.");
        }

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        await _brandAppService.UploadLogoBinaryAsync(id, ms.ToArray(), file.FileName);

        return NoContent();
    }
}