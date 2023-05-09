using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class Student
    {
        public Student()
        {
            Messages = new HashSet<Message>();
            Months = new HashSet<Month>();
        }

        public int StId { get; set; }
        public string StName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? RegistedDate { get; set; }
        //public bool PaymentState { get; set; }
        //public bool Available { get; set; }
        public int ClassId { get; set; }
        public string? IdentityUserId { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual AspNetUser? IdentityUser { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Month> Months { get; set; }
    }
}