using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Ofix.CarListingImages
{
    public interface ICarListingImageAppService : IApplicationService
    {
        Task<List<CarListingImageDto>> GetListAsync(Guid carListingId);

        Task SaveImagesAsync(Guid carListingId, List<SaveCarListingImageInput> images);
    }
}
