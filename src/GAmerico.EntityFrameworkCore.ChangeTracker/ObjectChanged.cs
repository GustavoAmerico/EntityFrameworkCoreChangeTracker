using System.Collections.Generic;
using System.Linq;

namespace GAmerico.EntityFrameworkCore.ChangeTracker
{
    /// <summary>This represents an object changed </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectChanged<T> where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectChanged{T}"/> class.</summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="properties">The properties.</param>
        public ObjectChanged(int eventId, string eventName, T entity, IEnumerable<PropertyChanged> properties)
        {
            EventId = eventId;
            EventName = eventName;
            Entity = entity;

            if (ReferenceEquals(null, properties))
            {
                Properties = new PropertyChanged[0];
            }
            else
            {
                var list = new List<PropertyChanged>(entity.GetType().GetProperties().Length);
                list.AddRange(properties);
                Properties = list.ToArray();
            }
        }

        /// <summary>Gets the entity.</summary>
        /// <value>The entity.</value>
        public T Entity { get; }

        /// <summary>Gets the event identifier.</summary>
        /// <value>The event identifier.</value>
        public int EventId { get; }

        /// <summary>Gets the name of the event.</summary>
        /// <value>The name of the event.</value>
        public string EventName { get; }

        /// <summary>Gets the properties.</summary>
        /// <value>The properties. </value>
        public IEnumerable<PropertyChanged> Properties { get; }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => $"{typeof(T).Name} was {EventName}";
    }
}