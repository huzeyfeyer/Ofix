using System;
using System.Collections.Generic;
using System.Text;

namespace Ofix.CarListingImages
{
    public class TempUploadedCarListingImageDto
    {
        public string TempFileToken { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}
