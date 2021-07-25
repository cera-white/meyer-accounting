using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class ServiceMetaData
    {
        [Required(ErrorMessage = "*Name is required")]
        [MaxLength(50, ErrorMessage = "Name must be less than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Summary is required")]
        [MaxLength(200, ErrorMessage = "Summary must be less than 200 characters.")]
        public string Summary { get; set; }

        [MaxLength(100, ErrorMessage = "Icon name must be less than 100 characters.")]
        public string Icon { get; set; }

        [Display(Name = "Visible?")]
        [Required]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(ServiceMetaData))]
    public partial class Service { }
}
