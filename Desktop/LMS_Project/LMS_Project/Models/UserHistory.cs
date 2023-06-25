

using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Models
{
    public class UserHistory
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime? CreatedDate { get; set; }


    }
}
