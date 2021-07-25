using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MeyerAccountingV2.EF
{
    class NewsletterMetaData
    {
        [Required(ErrorMessage = "*Title is required")]
        [MaxLength(50, ErrorMessage = "Title must be less than 50 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*Description is required")]
        [AllowHtml]
        public string Description { get; set; }

        [Display(Name = "Submitted")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DateSubmitted { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Download Link")]
        public string DownloadLink { get; set; }

        [MaxLength(500, ErrorMessage = "Tags must be less than 500 characters.")]
        public string Tags { get; set; }

        [Required]
        [Display(Name = "Visible?")]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(NewsletterMetaData))]
    public partial class Newsletter { }
}
