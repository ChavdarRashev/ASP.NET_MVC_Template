using DataCommon.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.Data
{
    public class FileDBContext : DbContext
    {
        public FileDBContext(DbContextOptions<FileDBContext> options) : base(options)
        {
        }

        public DbSet<FileRashev> Files { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
          

           builder.Entity<FileRashev>().HasQueryFilter(b => !b.IsDeleted);

           // base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public async Task<int> SaveChangesWithDateTimeAsync()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                //entityEntry.Property("ModifiedOn").CurrentValue = DateTime.Now;
                entityEntry.Property("ModifiedOn").CurrentValue = DateTime.Now;
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property("CreatedOn").CurrentValue = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}
