using System;
using System.Collections.Generic;

namespace GAmerico.EntityFrameworkCore.ChangeTracker
{
    /// <summary>Essa classe extende as funcionalidades da interface de tracker</summary>
    public static class EntityTrackerEnumerableExtensions
    {
        public static void BeginTrack(this IEnumerable<IEntityTracker> trackers, Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker changeTracker)
        {
            foreach (var entityTracker in trackers)
                entityTracker.BeginTrack(changeTracker);
        }

        public static void Commit(this IEnumerable<IEntityTracker> trackers)
        {
            foreach (var entityTracker in trackers)
                entityTracker.CommitTrack();
        }

        public static void NotifyError(this IEnumerable<IEntityTracker> trackers, Exception changeTracker)
        {
            foreach (var entityTracker in trackers)
                entityTracker.NotifyError(changeTracker);
        }
    }
}