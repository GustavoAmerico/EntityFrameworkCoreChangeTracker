using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GAmerico.EntityFrameworkCore.ChangeTracker
{

    /// <summary>Type responsible for monitoring a particular entity</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntitiesTracker<TEntity> : IEntityTracker,
        IObservable<IReadOnlyCollection<ObjectChanged<TEntity>>>, IDisposable where TEntity : class
    {
        private readonly List<IObserver<IReadOnlyCollection<ObjectChanged<TEntity>>>> _observers;

        /// <summary>List of properties to monitor</summary>
        private readonly HashSet<string> _properties = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        private readonly Queue<IReadOnlyCollection<ObjectChanged<TEntity>>> _transientItens = new Queue<IReadOnlyCollection<ObjectChanged<TEntity>>>();

        public EntitiesTracker(
            IEnumerable<IObserver<IReadOnlyCollection<ObjectChanged<TEntity>>>> observers, params Expression<Func<TEntity, object>>[] properties)
        {
            _observers = observers.ToList();
            if (!ReferenceEquals(null, properties))
            {
                foreach (var property in properties)
                {
                    var name = GetPropertyName(property);
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        _properties.Add(name);
                    }
                }
            }
        }

        /// <summary>Begins the track.</summary>
        /// <param name="provider">The provider.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public void BeginTrack(Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker changeTracker)
        {
            if (!_observers.Any()) return;
            var list = new List<ObjectChanged<TEntity>>(8);
            foreach (var objectChanged in changeTracker.SelectObjectsChanged<TEntity>(_properties))
                list.Add(objectChanged);
            _transientItens.Enqueue(list);
        }

        /// <summary>Commits the track for all observers</summary>
        public void CommitTrack()
        {
            if (ReferenceEquals(null, _transientItens) || !_transientItens.Any()) return;

            lock (_transientItens)
            {
                while (_transientItens.TryDequeue(out var objectChanged))
                {
                    //Percorre todos os observadores
                    foreach (var observer in _observers)
                    {
                        try
                        {
                            //Notifica a alteração do objeto
                            observer.OnNext(objectChanged);
                        }
                        catch (Exception e)
                        {
                            e.Data[typeof(TEntity)?.FullName ?? "TEntity"] = objectChanged;
                            observer.OnError(e);
                        }
                    }
                }
                _transientItens.Clear();
            }
        }

        public void Dispose()
        {
            foreach (var observer in _observers)
                observer.OnCompleted();
            _transientItens.Clear();
        }

        /// <summary>Notifies the error.</summary>
        /// <param name="exception">The exception.</param>
        public void NotifyError(Exception exception)
        {
            foreach (var observer in _observers)
                observer.OnError(exception);
        }

        public IDisposable Subscribe(IObserver<IReadOnlyCollection<ObjectChanged<TEntity>>> observer)
        {
            _observers.Add(observer);
            return new UnSubscribe(observer, _observers);
        }

        private static string GetPropertyName(Expression<Func<TEntity, object>> exp)
        {
            if (exp.Body is MemberExpression body) return body?.Member?.Name;
            var ubody = (UnaryExpression)exp.Body;
            body = ubody.Operand as MemberExpression;
            return body?.Member?.Name;
        }

        private class UnSubscribe : IDisposable
        {
            private readonly IObserver<IReadOnlyCollection<ObjectChanged<TEntity>>> _observer;
            private readonly List<IObserver<IReadOnlyCollection<ObjectChanged<TEntity>>>> _observers;

            public UnSubscribe(
                IObserver<IReadOnlyCollection<ObjectChanged<TEntity>>> observer,
                List<IObserver<IReadOnlyCollection<ObjectChanged<TEntity>>>> observers)
            {
                _observer = observer;
                _observers = observers;
            }

            public void Dispose()
            {
                _observers.Remove(_observer);
            }
        }
    }
}