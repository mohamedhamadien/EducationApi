using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class Login
    {
        public Login()
        {
            Students = new HashSet<Student>();
        }

        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;

        public virtual ICollection<Student> Students { get; set; }
    }
}
