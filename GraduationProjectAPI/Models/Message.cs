using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class Message
    {
        public int Mid { get; set; }
        public string Body { get; set; } = null!;
        public DateTime Date { get; set; }
        public int ChatId { get; set; }
        public int StId { get; set; }

        public virtual Chat Chat { get; set; } = null!;
        public virtual Student St { get; set; } = null!;
    }
}
