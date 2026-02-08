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

                var keyNames = entry.Metadata
                    .FindPrimaryKey()?
                    .Properties
                    .Select(p => p.Name)
                    .ToHashSet();

                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.ClrType != typeof(string))
                        continue;

                    if (property.Metadata.IsKey())
                        continue;

                    if (property.Metadata.IsForeignKey())
                        continue;

                    if (property.Metadata.GetColumnType()?.StartsWith("char") == true)
                        continue;

                    var value = property.CurrentValue as string;
                    if (value != null)
                        property.CurrentValue = value.Trim();
                }
            }
        }
    }
}