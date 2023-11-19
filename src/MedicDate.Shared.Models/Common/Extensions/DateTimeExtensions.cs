namespace MedicDate.Shared.Models.Common.Extensions;

public static class DateTimeExtensions
{
  public static int GetAge(this DateTime birthDate)
  {
    var now = DateTime.Today;
    var age = now.Year - birthDate.Year;
    if (birthDate > now.AddYears(-age))
      age--;
    return age;
  }

  public static bool IsTimeEquals(this DateTime firstDate, DateTime secondDate)
  {
    var firstTime = TimeOnly.FromDateTime(firstDate);
    var secondTime = TimeOnly.FromDateTime(secondDate);

    return firstTime.Hour == secondTime.Hour
           && firstTime.Minute == secondTime.Minute;
  }
}