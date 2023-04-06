using GraduationProjectAPI.Models;

namespace GraduationProjectAPI.Dtos
{
    public class CreateContentDto
    {
        public int ClassIdfk { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public string[] Images { get; set; } = null;
        public string[] Pdfs { get; set; } = null;
        public string[] Videos { get; set; } = null;
    }
}
