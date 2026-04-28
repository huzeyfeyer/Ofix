using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Content;

namespace Ofix.CarListingImages
{
    public class UploadTempCarListingImageInput
    {
        public IRemoteStreamContent File { get; set; } = default!;
    }
}
