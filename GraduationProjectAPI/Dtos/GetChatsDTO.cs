namespace GraduationProjectAPI.Dtos
{
    public class GetChatsDTO
    {
        public int? ID { get; set; }
        public string? Title { get; set; }
        public List<GetAllMessages>? AllMessages { get; set; }
    }
}
