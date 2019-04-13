using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace spy_detection.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Spy> Spies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Spy>()
                .Property(e => e.Code)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(new[] { ',' }).Select(num => int.Parse(num)).ToArray());
        }
    }
}
