using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class TestimonialMetaData
    {
        [Required(ErrorMessage = "*Author name is required")]
        [MaxLength(100, ErrorMessage = "Author name must be less than 100 characters.")]
        public string Author { get; set; }

        [Display(Name = "Company")]
        [MaxLength(100, ErrorMessage = "Company name must be less than 100 characters.")]
        public string AuthorTitle { get; set; }

        [Display(Name = "Submitted")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateSubmitted { get; set; }

        [Required(ErrorMessage = "*Comment is required")]
        [MaxLength(500, ErrorMessage = "Comment must be less than 500 characters.")]
        public string Comment { get; set; }

        [Display(Name = "Visible?")]
        [Required]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(TestimonialMetaData))]
    public partial class Testimonial { }
}
