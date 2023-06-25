using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace LMS_Project.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AppFile> File { get; set; }
        public DbSet<EventShow> eventshow { get; set; }
        public DbSet<UserHistory> userHistories { get; set; } 
        public DbSet<Tbl_Platform> tbl_platform { get; set; } 
        public DbSet<PostPlatform>? PostPlatforms { get; set; } 
        
    }
}
