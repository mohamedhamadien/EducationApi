using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class ContantVideo
    {
        public int Id { get; set; }
        public int? ContantIdfk { get; set; }
        public string? Path { get; set; }

        public virtual Contant? ContantIdfkNavigation { get; set; }
    }
}
