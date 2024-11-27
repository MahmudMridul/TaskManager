using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagerApi.Models;

namespace TaskManagerApi.Db
{
    public class TaskManagerContext : IdentityDbContext<User>
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> ops) : base(ops)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Suppress the specific warning
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Approver", NormalizedName = "APPROVER" }
            );
        }

    }
}
