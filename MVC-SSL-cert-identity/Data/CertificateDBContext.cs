using DataCommon.Model;
using Microsoft.EntityFrameworkCore;
using MVC_SSL_cert_identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace MVC_SSL_cert_identity.Data
{
    public class CertificateDBContext : DbContext
    {
        public CertificateDBContext(DbContextOptions<CertificateDBContext> options)
           : base(options)
        {
        }

       
        public DbSet<Certificate> Certificates { get; set; }

        //ToDo долните методи SaveChangesWithDateTime, SaveChangesWithDateTime, RemoveSoftAsync да се изнасат като services, започнато е в в проекта Services, но няма време към 12.04.2022 да се направи.
        public int SaveChangesWithDateTime()
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

            return base.SaveChanges();
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






        public Task RemoveSoftAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                entry.CurrentValues["IsDeleted"] = true;
                entry.CurrentValues["DeletedOn"] = DateTime.Now;
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: Да се автоматизира процеса за създаване на Query filters въз основа на  Entity-та , които наследяват IDeletableEntity
            modelBuilder.Entity<Certificate>().HasQueryFilter(b => !b.IsDeleted);           
        }


    }
}
