using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class ContentVideo
    {
        public int Id { get; set; }
        public string? Path { get; set; }
        public int ContentId { get; set; }

        public virtual Content Content { get; set; } = null!;
    }
}
