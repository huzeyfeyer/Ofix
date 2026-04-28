using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ofix.CarListingImages
{
    public interface ICarListingImageAppService : IApplicationService
    {
        Task<List<CarListingImageDto>> GetListAsync(Guid carListingId);

        Task SaveImagesAsync(Guid carListingId, List<SaveCarListingImageInput> images);
        Task<TempUploadedCarListingImageDto> UploadTempAsync(UploadTempCarListingImageInput input);

        Task RemoveTempAsync(string tempFileToken);
    }
}
