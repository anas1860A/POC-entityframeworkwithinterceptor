using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFrameworkDemo
{
    public class EventBrokerListener : ISaveChangesInterceptor
    {
        public void SaveChangesFailed(DbContextErrorEventData eventData)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            return 1;
        }

        public ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                Correction(entry.Entity);
            }
            return result;
        }

        public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        internal static void Correction(object entity)
        {
            var properties = entity.GetType().GetProperties().Where(p => p.PropertyType == typeof(String));

            foreach (var item in properties)
            {
                if (item.CanWrite)
                {
                    var result = item.GetValue(entity, null);
                    if (result != null)
                    {
                        result = result.ToString().Trim();
                        item.SetValue(entity, result, null);
                    }
                }
            }
        }
    }
}
