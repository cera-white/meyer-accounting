using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class BusinessHourMetaData
    {
        [Required(ErrorMessage = "*Day(s) is required")]
        [MaxLength(50, ErrorMessage = "Day(s) must be less than 50 characters.")]
        [Display(Name = "Day(s)")]
        public string Day { get; set; }

        [Required(ErrorMessage = "*Hours are required")]
        [MaxLength(50, ErrorMessage = "Hours must be less than 50 characters.")]
        public string Hours { get; set; }
    }

    [MetadataType(typeof(BusinessHourMetaData))]
    public partial class BusinessHour { }
}
