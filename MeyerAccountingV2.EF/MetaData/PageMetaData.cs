using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MeyerAccountingV2.EF
{
    class PageMetaData
    {
        [Required(ErrorMessage = "*Name is required")]
        [MaxLength(50, ErrorMessage = "Name must be less than 50 characters.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string Description { get; set; }

        [MaxLength(500, ErrorMessage = "Url must be less than 500 characters.")]
        public string Url { get; set; }

        [MaxLength(500, ErrorMessage = "Tags must be less than 500 characters.")]
        public string Tags { get; set; }

        [Required]
        [Display(Name = "Visible?")]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(PageMetaData))]
    public partial class Page { }
}
