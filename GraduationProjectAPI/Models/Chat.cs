using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class Chat
    {
        public Chat()
        {
            Classes = new HashSet<Class>();
            Messages = new HashSet<Message>();
        }

        public int ChatId { get; set; }
        public string? Title { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
