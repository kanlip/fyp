using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public partial class CorporateProfile
    {
        public string Cpid { get; set; }

        [Required(ErrorMessage = "The defination is required")]
        public string Defination { get; set; }

        [Required(ErrorMessage = "The Vision is required")]
        public string Vision { get; set; }

        [Required(ErrorMessage = "The Contact is required")]
        [RegularExpression(@"[89]\d{7}", ErrorMessage = "Invalid Phone Number")]
        public int Contact { get; set; }

        [Required(ErrorMessage = "The Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The AboutUs is required")]
        public string AboutUs { get; set; }

        [Required(ErrorMessage = "The Services is required")]
        public string Services { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Invalid Email address")]
        public string Email { get; set; }
        public string Fblink { get; set; }
        public string Twitlink { get; set; }
        public string Linkinlink { get; set; }
    }
}
