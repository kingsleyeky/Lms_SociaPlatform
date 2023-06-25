using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Models
{
    public class Account
    {
        [Key]
        public int PlatformId { get; set; } 
        public string? UserEmail { get; set; }
        public string? Name { get; set; }
        //public string? PlatformName { get; set; } 
        public DateTime? CreatedDate { get; set; }
    }
}
