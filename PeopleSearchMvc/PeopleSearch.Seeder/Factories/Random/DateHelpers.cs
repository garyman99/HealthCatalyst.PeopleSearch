using System;

namespace PeopleSearch.Seeder.PersonFactories.Random
{
    public static class DateHelpers
    {
        public static DateTime RandomDate(int minYear, int maxYear, int? seed = null)
        {
            var random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
            var monthOfBirth = new DateTime(random.Next(1950, 2006), random.Next(1, 12), 1);
            var dayOfBirth = random.Next(1, monthOfBirth.AddMonths(1).AddDays(-1).Day);
            return new DateTime(monthOfBirth.Year, monthOfBirth.Month, dayOfBirth);
        }
    }
}