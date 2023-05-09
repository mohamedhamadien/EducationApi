using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class Chat
    {
        public Chat()
        {
            Messages = new HashSet<Message>();
        }

        public int ChatId { get; set; }
        public string? Title { get; set; }
        public int ClassIdfk { get; set; }

        public virtual Class ClassIdfkNavigation { get; set; } = null!;
        public virtual ICollection<Message> Messages { get; set; }
    }
}
