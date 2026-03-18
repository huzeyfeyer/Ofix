using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ofix.FeatureCategories
{
    public class CreateUpdateFeatureCategoryDto
    {
        public string Name { get; set; }
        public string? Icon { get; set; }
        public int OrderNo { get; set; }
        public bool Status { get; set; }
    }
}
