using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Models
{
    public class EventShow
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? ThemeColor { get; set; }
        public Boolean IsFullDay { get; set; }
        public string? Location { get; set; }
    }
}
