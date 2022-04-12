using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    class DBOperation : IDBOperation
    {
       /* 
        
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

        */
    }
}
