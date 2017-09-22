using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public partial class Appointment
    {
        public Appointment()
        {
            Feedback = new HashSet<Feedback>();
        }

        public int AppId { get; set; }

        [Required(ErrorMessage ="Password cannot be blank!")]
        public DateTime AppDate { get; set; }
        public string AppStatus { get; set; }
        public string AppDetail { get; set; }
        public string AppTitle { get; set; }
        public string AppColor { get; set; }
        public int CusId { get; set; }
        public int StaffId { get; set; }

        public virtual ICollection<Feedback> Feedback { get; set; }
        public virtual Customer Cus { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
