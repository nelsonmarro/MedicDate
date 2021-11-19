using System.Reflection;

namespace MedicDate.Utility.Validators;

public static class PropertyValidator
{
  public static bool IsValidProperty<T>(
    string propertyName,
    bool throwExceptionIfNotFound = true)
  {
    var prop = typeof(T).GetProperty(
      propertyName,
      BindingFlags.IgnoreCase |
      BindingFlags.Public |
      BindingFlags.Instance);
    if (prop == null && throwExceptionIfNotFound)
      throw new NotSupportedException(
        $"ERROR: Property '{propertyName}' does not exist."
      );
    return prop != null;
  }
}