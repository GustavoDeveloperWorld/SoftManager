using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftManager.Models;

namespace SoftManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<UserTask> Tasks { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
