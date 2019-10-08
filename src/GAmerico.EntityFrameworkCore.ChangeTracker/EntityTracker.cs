using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GAmerico.EntityFrameworkCore.ChangeTracker
{
  /// <summary>Type responsible for monitoring a particular entity</summary>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  public class EntityTracker<TEntity> : IEntityTracker, IObservable<ObjectChanged<TEntity>>, IDisposable where TEntity : class
  {
    private readonly List<IObserver<ObjectChanged<TEntity>>> _observers;

    /// <summary>List of properties to monitor</summary>
    private readonly HashSet<string> _properties = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

    private List<ObjectChanged<TEntity>> _transientItens = new List<ObjectChanged<TEntity>>();

    public EntityTracker(IEnumerable<IObserver<ObjectChanged<TEntity>>> observers, params Expression<Func<TEntity, object>>[] properties)
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
      _transientItens.AddRange(changeTracker.SelectObjectsChanged<TEntity>(_properties));
    }

    /// <summary>Commits the track for all observers</summary>
    public void CommitTrack()
    {
      if (ReferenceEquals(null, _transientItens)) return;

      lock (_transientItens)
      {
        //Percorre todos os observadores
        foreach (var observer in _observers)
        {
          //Percorre todos os itens alterados
          foreach (var objectChanged in _transientItens)
          {
            try
            {
              //Notifica a alteração do objeto
              observer.OnNext(objectChanged);
            }
            catch (Exception e)
            {
              e.Data.Add(objectChanged.Entity, objectChanged);
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
    }

    /// <summary>Notifies the error.</summary>
    /// <param name="exception">The exception.</param>
    public void NotifyError(Exception exception)
    {
      foreach (var observer in _observers)
        observer.OnError(exception);
    }

    public IDisposable Subscribe(IObserver<ObjectChanged<TEntity>> observer)
    {
      _observers.Add(observer);
      return new UnSubscribe(observer, _observers);
    }

    private static string GetPropertyName(Expression<Func<TEntity, object>> exp)
    {
      var body = exp.Body as MemberExpression;

      if (body != null) return body?.Member?.Name;
      var ubody = (UnaryExpression)exp.Body;
      body = ubody.Operand as MemberExpression;
      return body?.Member?.Name;
    }

    private class UnSubscribe : IDisposable
    {
      private readonly IObserver<ObjectChanged<TEntity>> _observer;
      private readonly List<IObserver<ObjectChanged<TEntity>>> _observers;

      public UnSubscribe(IObserver<ObjectChanged<TEntity>> observer, List<IObserver<ObjectChanged<TEntity>>> observers)
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