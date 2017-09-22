using System;
using System.Collections.Generic;

namespace FYP.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public DateTime FeedBackTime { get; set; }
        public string CustomerComment { get; set; }
        public string StaffComment { get; set; }
        public int? AId { get; set; }
        public int? CId { get; set; }
        public int? SId { get; set; }

        public virtual Appointment A { get; set; }
        public virtual Customer C { get; set; }
    }
}
