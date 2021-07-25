using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MeyerAccountingV2.EF
{
    class SectionMetaData
    {
        [Required(ErrorMessage = "*Name is required")]
        [MaxLength(50, ErrorMessage = "Name must be less than 50 characters.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string Description { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Visible?")]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(SectionMetaData))]
    public partial class Section { }
}
