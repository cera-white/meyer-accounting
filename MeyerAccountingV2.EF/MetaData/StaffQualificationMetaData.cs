using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class StaffQualificationMetaData
    {
        [Required(ErrorMessage = "*Description is required")]
        [MaxLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string Description { get; set; }
    }

    [MetadataType(typeof(StaffQualificationMetaData))]
    public partial class StaffQualification { }
}
