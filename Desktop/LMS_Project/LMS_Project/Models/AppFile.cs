using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Models
{
    public class AppFile
    {
        [Key]
        public int Id { get; set; }
        public string? FileName { get; set; }
        public byte[]? Content { get; set; }
    }
}
