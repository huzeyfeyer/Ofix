using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ofix.CarListings
{
    public interface ICarListingAppService :
        ICrudAppService<
            CarListingDto,
            Guid,
            CarListingListInput,
            CreateUpdateCarListingDto>
    {
        Task<CarListingDto> GetPublishedDetailAsync(Guid id);
    }
}
