using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ABB.Infrastructure
{
    public class TrimStringSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            TrimEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            TrimEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void TrimEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State is not (EntityState.Added or EntityState.Modified))
                    continue;

                foreach (var property in entry.Properties)
                {
                    // 1. Only target strings
                    if (property.Metadata.ClrType != typeof(string))
                        continue;

                    // 2. Skip Keys and Foreign Keys
                    if (property.Metadata.IsKey() || property.Metadata.IsForeignKey())
                        continue;

                    // 3. Get the value safely
                    var value = property.CurrentValue as string;

                    // 4. The Fix: Only trim if the value is NOT null
                    // This ensures null remains null.
                    if (value != null)
                    {
                        var trimmedValue = value.Trim();
                        
                        // Optional: If you want to convert "   " to null instead of "", 
                        // use string.IsNullOrEmpty(trimmedValue) ? null : trimmedValue;
                        property.CurrentValue = trimmedValue;
                    }
                }
            }
        }
    }
}