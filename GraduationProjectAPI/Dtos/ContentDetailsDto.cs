namespace GraduationProjectAPI.Dtos
{
    public class ContentDetailsDto
    {
        public int Id { get; set; }
        public int ClassIdfk { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public string ClassName { get; set; }
    }
}
