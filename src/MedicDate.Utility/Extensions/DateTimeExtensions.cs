using System;

namespace MedicDate.Utility.Extensions
{
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
    }
}