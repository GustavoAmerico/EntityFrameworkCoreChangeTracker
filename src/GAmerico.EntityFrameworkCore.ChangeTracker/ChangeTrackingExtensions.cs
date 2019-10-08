using GAmerico.EntityFrameworkCore.ChangeTracker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.ChangeTracking
{
  /// <summary>Essa classe é responsável por extender as funcionalidades do <see cref="ChangeTracker"/></summary>
  public static class ChangeTrackingExtensions
  {
    /// <summary>Obtém a lista de status que são ignorados pelo rastreador</summary>
    public static EntityState[] IgnoreStates = new[] { EntityState.Unchanged, EntityState.Detached };

    /// <summary>Selects the objects changed.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entries">The entries.</param>
    /// <param name="properties">The properties.</param>
    /// <returns></returns>
    public static IEnumerable<ObjectChanged<T>> SelectObjectsChanged<T>(this ChangeTracker entries, ICollection<string> properties) where T : class
    {
      if (entries?.HasChanges() != true) yield break;
      foreach (var entry in entries.Entries<T>().Where(e => !IgnoreStates.Contains(e.State)))
      {
        var propertiesChanged = entry.SelectPropertiesChanged(properties);
        yield return new ObjectChanged<T>(((int)entry.State), entry.State.ToString(), entry.Entity, propertiesChanged);
      }
    }

    /// <summary>Gets the properties changed for an object</summary>
    /// <param name="entry">The entry.</param>
    /// <param name="properties">The properties for filters.</param>
    public static IEnumerable<PropertyChanged> SelectPropertiesChanged(this EntityEntry entry, ICollection<string> properties)
    {
      return from entryProperty in entry.Properties.Where(a => a.IsModified)
             where !properties.Any() || properties.Contains(entryProperty.Metadata.Name)
             select new PropertyChanged(entryProperty.Metadata.Name, entryProperty.CurrentValue, entryProperty.OriginalValue);
    }
  }
}