namespace GraduationProjectAPI.Dtos
{
    public class GetAllMessages
    {
        public int M_ID { get; set; }
        public string? M_Body { get; set; }
        public DateTime? M_Date { get; set; }
        public int? St_ID { get; set; }
        public string? StName { get; set; }
    }
}
