using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace GAmerico.EntityFrameworkCore.ChangeTracker
{
  public class DbContextTracker : DbContext
  {
    public DbContextTracker(DbContextOptions options) : base(options)
    {
    }

    protected DbContextTracker() : base()
    {
    }

    /// <summary>Saves all changes made in this context to the database.</summary>
    /// <returns>The number of state entries written to the database.</returns>
    /// <remarks>This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
    /// changes to entity instances before saving to the underlying database. This can be disabled via
    /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
    /// </remarks>
    public override int SaveChanges()
    {
      var trackers = this.GetInfrastructure()
        .GetServices<IEntityTracker>()
        .ToArray();

      try
      {
        trackers.BeginTrack(this.ChangeTracker);
        var result = base.SaveChanges();
        trackers.Commit();
        return result;
      }
      catch (Exception e)
      {
        foreach (var entityTracker in trackers)
          entityTracker.NotifyError(e);
        throw;
      }
    }
  }
}