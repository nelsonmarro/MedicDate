using System;

namespace MedicDate.Utility
{
    public static class AgeCalculator
    {
        public static int GetAge(DateTime birthDate)
        {
            var now = DateTime.Today;
            var age = now.Year - birthDate.Year;
            if (birthDate.AddYears(age) > now)
                age--;
            return age;
        }
    }
}