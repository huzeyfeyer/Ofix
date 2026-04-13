using Ofix.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ofix.SubModels
{
    public class CreateUpdateSubModelDto
    {
        public Guid ModelId { get; set; }

        [Required]
        [StringLength(SubModelConsts.MaxNameLength)]
        public string Name { get; set; } = string.Empty;

        public int OrderNo { get; set; }

        public ListingStatus ListingStatus { get; set; } = ListingStatus.Draft;

        [StringLength(SubModelConsts.MaxSlugLength)]
        public string Slug { get; set; } = string.Empty;
    }

}