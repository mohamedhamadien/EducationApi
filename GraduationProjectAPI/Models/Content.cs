using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class Content
    {
        public Content()
        {
            ContentImages = new HashSet<ContentImage>();
            ContentPdfs = new HashSet<ContentPdf>();
            ContentVideos = new HashSet<ContentVideo>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public int ClassId { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual ICollection<ContentImage> ContentImages { get; set; }
        public virtual ICollection<ContentPdf> ContentPdfs { get; set; }
        public virtual ICollection<ContentVideo> ContentVideos { get; set; }
    }
}
