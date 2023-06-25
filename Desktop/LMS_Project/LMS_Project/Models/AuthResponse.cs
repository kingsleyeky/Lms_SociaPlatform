namespace LMS_Project.Models
{
    public class AuthResponse
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? FirstName { get; set; }
    }
}
