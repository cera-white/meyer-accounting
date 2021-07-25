using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class LinkMetaData
    {
        [Required(ErrorMessage = "*Display Text is required")]
        [MaxLength(50, ErrorMessage = "Display Text must be less than 50 characters.")]
        [Display(Name = "Display Text")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*URL is required")]
        [MaxLength(500, ErrorMessage = "URL must be less than 500 characters.")]
        [Display(Name = "URL")]
        public string Url { get; set; }

        [Required(ErrorMessage = "*Category is required")]
        [Display(Name = "Category")]
        public int LinkTypeId { get; set; }

        [Display(Name = "Visible?")]
        [Required]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(LinkMetaData))]
    public partial class Link { }
}
