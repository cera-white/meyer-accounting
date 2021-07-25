using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class CommentMetaData
    {
        [Required(ErrorMessage = "*Name is required")]
        [MaxLength(500, ErrorMessage = "Name must be less than 500 characters.")]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*Comment is required")]
        [MaxLength(1000, ErrorMessage = "Comment must be less than 1000 characters.")]
        [Display(Name = "Comment")]
        public string Comment1 { get; set; }

        [Required]
        [Display(Name = "Visible?")]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(CommentMetaData))]
    public partial class Comment { }
}
