using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GAmerico.EntityFrameworkCore.ChangeTracker
{
    public class DbContextTracker : DbContext
    {
        private IEnumerable<IEntityTracker> _observers;

        public DbContextTracker(DbContextOptions options, IEnumerable<IEntityTracker> observers) : base(options)
        {
            _observers = observers;
        }

        protected DbContextTracker(IEnumerable<IEntityTracker> observers) : base()
        {
            _observers = observers;
        }

        /// <summary>Saves all changes made in this context to the database.</summary>
        /// <returns>The number of state entries written to the database.</returns>
        /// <remarks>This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </remarks>
        public override int SaveChanges()
        {
            try
            {
                _observers?.BeginTrack(this.ChangeTracker);
                var result = base.SaveChanges();
                _observers?.Commit();
                return result;
            }
            catch (Exception e)
            {
                if (!ReferenceEquals(null, _observers))
                {
                    foreach (var entityTracker in _observers)
                        entityTracker.NotifyError(e);
                }
                throw;
            }
        }
    }
}