using System;
using System.Collections.Generic;

namespace FYP.Models
{
    public partial class CustomerBackup
    {
        public string BackUpId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public int PhoneNo { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public byte[] Password { get; set; }
        public int? HomeNo { get; set; }
        public DateTime Dob { get; set; }
        public string NextofKin { get; set; }
        public int KinNo { get; set; }
        public string CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
