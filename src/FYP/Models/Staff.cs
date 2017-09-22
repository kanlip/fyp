using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public partial class Staff
    {

        public int StaffId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"[89]\d{7}", ErrorMessage = "Invalid phone number format")]
        public int PhoneNo { get; set; }
        
        public byte[] Password { get; set; }

        

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        //[Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date")]
        [Required(ErrorMessage = "The Date of birth is required")]
        public DateTime Dob { get; set; }


    }
}
