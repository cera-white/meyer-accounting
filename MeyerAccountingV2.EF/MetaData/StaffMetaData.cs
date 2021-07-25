using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class StaffMetaData
    {
        [Required(ErrorMessage = "*First Name is required")]
        [MaxLength(50, ErrorMessage = "First Name must be less than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Middle name must be less than 50 characters.")]
        [Display(Name = "Middle")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "*Last Name is required")]
        [MaxLength(50, ErrorMessage = "Last Name must be less than 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*Title is required")]
        [MaxLength(100, ErrorMessage = "Title must be less than 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*Rank is required")]
        public int Rank { get; set; }

        [Display(Name = "Visible?")]
        [Required]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(StaffMetaData))]
    public partial class Staff
    {
        public string FullName
        {
            get
            {
                return string.Format("{0} {1} {2}", this.FirstName, this.MiddleName, this.LastName);
            }
        }
    }
}
