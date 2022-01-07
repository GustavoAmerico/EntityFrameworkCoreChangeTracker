using System;

namespace GAmerico.EntityFrameworkCore.ChangeTracker
{
    /// <summary>Type responsible for monitoring a particular entity</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IEntityTracker
    {
        /// <summary>Begins the track change on database</summary>
        /// <param name="changeTracker">The change tracker.</param>
        void BeginTrack(Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker changeTracker);

        /// <summary>Commits the track for all observers</summary>
        void CommitTrack();

        /// <summary>Notifies the error.</summary>
        /// <param name="exception">The exception.</param>
        void NotifyError(Exception exception);
    }
}