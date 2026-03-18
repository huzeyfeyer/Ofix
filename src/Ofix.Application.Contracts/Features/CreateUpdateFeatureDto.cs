using System;
using System.Collections.Generic;
using System.Text;

namespace Ofix.Features
{
    public class CreateUpdateFeatureDto
    {
        public Guid FeatureCategoryId { get; set; }
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public bool Status { get; set; }
    }

}
