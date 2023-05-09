using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class Class
    {
        public Class()
        {
            Contents = new HashSet<Content>();
            Students = new HashSet<Student>();
        }

        public int ClassId { get; set; }
        public string Title { get; set; } = null!;

        public virtual Chat? Chat { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
