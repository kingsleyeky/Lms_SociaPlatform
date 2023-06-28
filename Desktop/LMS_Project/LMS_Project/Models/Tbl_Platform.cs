

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Project.Models
{
    public class Tbl_Platform
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; } 
        //public string? Title { get; set; }        
    }

    public class PostPlatform
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(PlatformId))]
        public Tbl_Platform? Platform { get; set; }
        public int PlatformId { get; set; } 

        [ForeignKey(nameof(PostId))]
        public PostMedia? Post { get; set; }
        public int PostId { get; set; }

    }
}
