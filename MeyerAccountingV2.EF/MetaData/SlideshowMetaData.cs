using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class SlideshowMetaData
    {
        [Required(ErrorMessage = "*Title is required")]
        [MaxLength(50, ErrorMessage = "Title must be less than 50 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*Description is required")]
        [MaxLength(100, ErrorMessage = "Description must be less than 100 characters.")]
        public string Description { get; set; }

        [Display(Name = "Right Aligned?")]
        public bool IsRightAligned { get; set; }

        [Display(Name = "Visible?")]
        [Required]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(SlideshowMetaData))]
    public partial class Slideshow { }
}
