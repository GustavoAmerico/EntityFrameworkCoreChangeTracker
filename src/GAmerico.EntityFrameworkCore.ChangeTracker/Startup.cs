using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;

namespace GAmerico.EntityFrameworkCore.ChangeTracker
{
    /// <summary>This class supports the installation of the project lib</summary>
    public static class Startup
    {
        /// <summary>Add an object change tracker for <see cref="T"/></summary>
        /// <typeparam name="T">Object type for track</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="properties">The properties.</param>
        public static IServiceCollection AddEntityFrameworkTracker<T>(this IServiceCollection services, params Expression<Func<T, object>>[] properties) where T : class
        {
            return services
              .AddScoped<EntityTracker<T>>(p => new EntityTracker<T>(p.GetServices<IObserver<ObjectChanged<T>>>(), properties))
              .AddScoped<IEntityTracker>(a => a.GetService<EntityTracker<T>>())
              .AddScoped<IObservable<ObjectChanged<T>>>(a => a.GetService<EntityTracker<T>>());
        }

        /// <summary>Add an object change tracker for <see cref="T"/></summary>
        /// <typeparam name="T">Object type for track</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="properties">The properties.</param>
        public static IServiceCollection AddEntityFrameworkTracker<T>(this IServiceCollection services, Func<Expression<Func<T, object>>[]> properties) where T : class
        {
            return services
              .AddScoped<EntityTracker<T>>(p => new EntityTracker<T>(p.GetServices<IObserver<ObjectChanged<T>>>(), properties?.Invoke()))
              .AddScoped<IEntityTracker>(a => a.GetService<EntityTracker<T>>())
              .AddScoped<IObservable<ObjectChanged<T>>>(a => a.GetService<EntityTracker<T>>());
        }

        /// <summary>Add an object change tracker for <see cref="T"/></summary>
        /// <typeparam name="T">Object type for track</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="properties">The properties.</param>
        public static IServiceCollection AddEntityFrameworkTracker<T, TObserver>(this IServiceCollection services)
            where T : class
            where TObserver : class, IObserver<ObjectChanged<T>>
        {
            return services
                .AddScoped<EntityTracker<T>>(p => new EntityTracker<T>(p.GetServices<IObserver<ObjectChanged<T>>>()))
                .AddScoped<IEntityTracker>(a => a.GetService<EntityTracker<T>>())
                .AddScoped<IObservable<ObjectChanged<T>>>(a => a.GetService<EntityTracker<T>>())
                .AddScoped<IObserver<ObjectChanged<T>>, TObserver>();
        }

        /// <summary>Add an object change tracker for <see cref="T"/></summary>
        /// <typeparam name="T">Object type for track</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="properties">The properties.</param>
        public static IServiceCollection AddEntityFrameworkTracker<T>(this IServiceCollection services, Func<IServiceProvider, IObserver<ObjectChanged<T>>> creator) where T : class
        {
            return services
                .AddScoped<EntityTracker<T>>(p => new EntityTracker<T>(p.GetServices<IObserver<ObjectChanged<T>>>()))
                .AddScoped<IEntityTracker>(a => a.GetService<EntityTracker<T>>())
                .AddScoped<IObservable<ObjectChanged<T>>>(a => a.GetService<EntityTracker<T>>())
                .AddScoped<IObserver<ObjectChanged<T>>>(creator);
        }

        /// <summary>Adds the track observer on DI.</summary>
        /// <param name="services">The services.</param>
        /// <param name="creator">The creator.</param>
        public static IServiceCollection AddTrackObserver<T, TObserver>(this IServiceCollection services)
          where T : class
    where TObserver : class, IObserver<ObjectChanged<T>>
        {
            return services.AddScoped<IObserver<ObjectChanged<T>>, TObserver>();
        }

        /// <summary>Adds the track observer on DI.</summary>
        /// <param name="services">The services.</param>
        /// <param name="creator">The creator.</param>
        public static IServiceCollection AddTrackObserver<T>(this IServiceCollection services, Func<IServiceProvider, IObserver<ObjectChanged<T>>> creator)
          where T : class
        {
            return services.AddScoped<IObserver<ObjectChanged<T>>>(creator);
        }
    }
}