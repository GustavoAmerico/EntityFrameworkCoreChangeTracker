using System;
using System.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GAmerico.EntityFrameworkCore.ChangeTracker
{
    /// <summary>Essa classe representa as alterações feitas em uma propriedade</summary>
    public class PropertyChanged
    {
        public PropertyChanged(string propertyName, object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
            PropertyName = propertyName;
        }

        public PropertyChanged(PropertyEntry entry)
        {
            if (ReferenceEquals(null, entry)) return;
            PropertyName = entry.Metadata.Name;
            OldValue = entry.OriginalValue;
            NewValue = entry.CurrentValue;
        }

        /// <summary>Gets the current value.</summary>
        /// <value>The current value.</value>
        public object NewValue { get; private set; }

        /// <summary>Gets the old value.</summary>
        /// <value>The old value.</value>
        public object OldValue { get; private set; }

        /// <summary>Gets the name of the property.</summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; private set; }

        public static implicit operator (string PropertyName, object NewValue, object OldValue)(PropertyChanged value)
        {
            return (value.PropertyName, value.OldValue, value.NewValue);
        }

        public static implicit operator PropertyChanged((string PropertyName, object NewValue, object OldValue) value)
        {
            return new PropertyChanged(value.PropertyName, value.OldValue, value.NewValue);
        }


        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{PropertyName} change from {OldValue} to {NewValue}";
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PropertyChanged pc)) return false;
            return String.Equals(pc.PropertyName, PropertyName, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => PropertyName?.GetHashCode() ?? 0;
    }
}