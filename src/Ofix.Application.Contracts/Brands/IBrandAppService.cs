using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace Ofix.Brands
{
    public interface IBrandAppService : ICrudAppService<
        BrandDto, //Used to show brands
        Guid, //Primary key of the brand entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBrandDto> //Used to create/update a brand
    {
    }
}
