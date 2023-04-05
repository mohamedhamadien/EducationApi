using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class Class
    {
        public Class()
        {
            Contants = new HashSet<Contant>();
            Students = new HashSet<Student>();
        }

        public int ClassId { get; set; }
        public string Title { get; set; } = null!;
        public int? ChatIdfk { get; set; }

        public virtual Chat? ChatIdfkNavigation { get; set; }
        public virtual ICollection<Contant> Contants { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
