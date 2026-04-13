using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using System.Threading.Tasks;

namespace Ofix.Brands
{
    public interface IBrandAppService : ICrudAppService<
        BrandDto, //Used to show brands
        Guid, //Primary key of the brand entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBrandDto> //Used to create/update a brand
    {
        Task UploadLogoBinaryAsync(Guid id, byte[] fileBytes, string fileName);
        Task<IRemoteStreamContent?> GetLogoAsync(Guid id);
    }
}
