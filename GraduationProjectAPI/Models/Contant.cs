using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.Models
{
    public partial class Contant
    {
        public Contant()
        {
            ContantImages = new HashSet<ContantImage>();
            ContantPdfs = new HashSet<ContantPdf>();
            ContantVideos = new HashSet<ContantVideo>();
        }

        public int Id { get; set; }
        public int ClassIdfk { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }

        public virtual Class ClassIdfkNavigation { get; set; } = null!;
        public virtual ICollection<ContantImage> ContantImages { get; set; }
        public virtual ICollection<ContantPdf> ContantPdfs { get; set; }
        public virtual ICollection<ContantVideo> ContantVideos { get; set; }
    }
}
