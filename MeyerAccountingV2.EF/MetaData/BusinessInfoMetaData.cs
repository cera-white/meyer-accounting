using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class BusinessInfoMetaData
    {
        [Required(ErrorMessage = "*Name is required")]
        [MaxLength(50, ErrorMessage = "Name must be less than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Value is required")]
        [MaxLength(500, ErrorMessage = "Value must be less than 500 characters.")]
        public string Value { get; set; }
    }

    [MetadataType(typeof(BusinessInfoMetaData))]
    public partial class BusinessInfo { }
}
