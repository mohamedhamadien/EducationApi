using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class Message
    {
        public int MId { get; set; }
        public string Body { get; set; } = null!;
        public DateTime Date { get; set; }
        public int? ChatIdfk { get; set; }
        public int? StIdfk { get; set; }

        public virtual Chat? ChatIdfkNavigation { get; set; }
        public virtual Student? StIdfkNavigation { get; set; }
    }
}
