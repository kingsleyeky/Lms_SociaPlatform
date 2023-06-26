using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Models
{
    public class PostMedia
    {
        [Key]
        public int Id { get; set; }
        public string? UserEmail { get; set; }
        public DateTime? CreatedDate { get; set; }

        [Required]
        public string? Content { get; set; } 

        [Required]
        public string? Title { get; set; }

        public List<PostPlatform>? PostPlatforms { get; set; }

        [Required]
        public string? VideoLink { get; set; }
        [Required]
        public string? ImagePath { get; set; }

        [Required]
        public DateTime? ScheduledTime { get; set; }
       
    }
}
