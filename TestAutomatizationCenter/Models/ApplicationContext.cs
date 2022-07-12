using Microsoft.EntityFrameworkCore;

namespace TestAutomatizationCenter.Models
{
    public class ApplicationContext : DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Message> Messages { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
    }
}
