using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "*Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Email address is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "*Message is required")]
        public string Message { get; set; }
    }
}