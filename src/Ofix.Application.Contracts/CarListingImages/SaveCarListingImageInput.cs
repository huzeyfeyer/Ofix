using System;
using System.Collections.Generic;
using System.Text;

namespace Ofix.CarListingImages
{
    public class SaveCarListingImageInput
    {
        public Guid? ExistingImageId { get; set; }

        public string TempFileToken { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public bool IsCover { get; set; }

        public bool IsDeleted { get; set; }
    }
}
