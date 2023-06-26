using LMS_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.ViewModel
{
    public class PostMediaDTO 
    {
        public int Id { get; set; }
        [Required]
        public string? Content { get; set; }

        [Required]
        public string? Title { get; set; } 

        public string? Name { get; set; }
        [Required]
        public string? VideoLink { get; set; }

        [Required]
        public DateTime ScheduledTime { get; set; }
      
        public List<int>? PlatformIds { get; set; }
    }
}
