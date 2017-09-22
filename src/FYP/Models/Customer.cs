using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Appointment = new HashSet<Appointment>();
            Feedback = new HashSet<Feedback>();
        }

        public int CustomerId { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage ="Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage ="Phone number is required")]
        public int PhoneNo { get; set; }

        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public int? HomeNo { get; set; }
        public DateTime Dob { get; set; }
        public string NextofKin { get; set; }
        
        
        public int KinNo { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<Feedback> Feedback { get; set; }
    }
}
