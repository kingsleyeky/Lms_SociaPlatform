namespace LMS_Project.ViewModel
{
    public class UserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string? OldPassword { get; set; }
        public string? ComfirmPassword { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
