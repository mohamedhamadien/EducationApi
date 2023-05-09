namespace GraduationProjectAPI.Dtos
{
    public class CreateStudentDTO
    {
        
        public string StName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? RegistedDate { get; set; }
        //public bool PaymentState { get; set; }
        //public bool Available { get; set; }
        public int ClassIdfk { get; set; }
        public string UserNameFk { get; set; } = null!;
        public string Password { get; set; } = null!;
        
    }
}
