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
        public static IReadOnlyCollection<ObjectChanged<T>> SelectObjectsChanged<T>(this ChangeTracker entries, ICollection<string> properties) where T : class
        {
            var itens = new List<ObjectChanged<T>>();
            if (entries?.HasChanges() != true) return itens;
            foreach (var entry in entries.Entries<T>().Where(e => !IgnoreStates.Contains(e.State)))
            {
                var propertiesChanged = entry.SelectPropertiesChanged(properties);
                if (!propertiesChanged.Any())
                    continue;
                var oc = new ObjectChanged<T>(((int)entry.State), entry.State.ToString(), entry.Entity, propertiesChanged);
                itens.Add(oc);
            }

            return itens;
        }

        /// <summary>Gets the properties changed for an object</summary>
        /// <param name="entry">The entry.</param>
        /// <param name="properties">The properties for filters.</param>
        public static IReadOnlyCollection<PropertyChanged> SelectPropertiesChanged(this EntityEntry entry, ICollection<string> properties)
        {
            var propertiesToLogger = entry
                .Properties
                .ToList();

            if (properties.Any())
            {
                propertiesToLogger = propertiesToLogger
                    .Where(prop => properties.Contains(prop.Metadata?.Name, StringComparer.CurrentCultureIgnoreCase))
                     .ToList();
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
            {
                return propertiesToLogger
                    .Select(s => new PropertyChanged(s))
                    .ToList();
            }

            propertiesToLogger
              .RemoveAll(entryProperty => EqualityComparer<object>.Default.Equals(entryProperty.CurrentValue, entryProperty.OriginalValue));

            return propertiesToLogger
                .Select(entryProperty => new PropertyChanged(entryProperty))
                .ToList();
        }
    }
}