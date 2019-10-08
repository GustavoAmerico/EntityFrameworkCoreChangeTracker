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

    /// <summary>Gets the current value.</summary>
    /// <value>The current value.</value>
    public object NewValue { get; }

    /// <summary>Gets the old value.</summary>
    /// <value>The old value.</value>
    public object OldValue { get; }

    /// <summary>Gets the name of the property.</summary>
    /// <value>The name of the property.</value>
    public string PropertyName { get; }

    public static implicit operator (string PropertyName, object NewValue, object OldValue) (PropertyChanged value)
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
  }
}