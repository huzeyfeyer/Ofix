using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Application.Dtos;

namespace Ofix.Brands
{
    public class BrandAppService : CrudAppService<
        Brand,BrandDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateBrandDto>, IBrandAppService
    {
        public BrandAppService(IRepository<Brand, Guid> repository) :base(repository)
        {
        }
    }
}
