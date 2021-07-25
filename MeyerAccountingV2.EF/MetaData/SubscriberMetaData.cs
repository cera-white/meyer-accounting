using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class SubscriberMetaData
    {
        [Required(ErrorMessage = "*Email address is required")]
        [MaxLength(150, ErrorMessage = "Email address must be less than 150 characters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Email Verified")]
        public bool EmailConfirmed { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(SubscriberMetaData))]
    public partial class Subscriber { }
}
