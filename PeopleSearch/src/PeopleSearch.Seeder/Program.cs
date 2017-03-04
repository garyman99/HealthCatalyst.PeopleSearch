using System;
using PeopleSearch.Contracts;

namespace PeopleSearch.Seeder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Seeder started...");
            try
            {
                var personPublisher = new PersonPublisher();

                personPublisher.Publish();

                Console.Write("Seeder Finished");
            }
            catch (Exception unhandledException)
            {
                Console.WriteLine($"Unhandled Excpetion in Seeder.  Details: \r\n{unhandledException}");
                throw;
            }

            Console.WriteLine("Press any key to exit");
            Console.Read();
            Environment.Exit(0);
        }
    }

    public class PersonPublisher
    {
        private readonly IRandomPersonFactory _randomPersonFactory;

        public PersonPublisher(IRandomPersonFactory randomPersonFactory)
        {
            _randomPersonFactory = randomPersonFactory;
        }

        public void Publish()
        {
            for (int i = 0; i < 1000; i++)
            {
                var newPerson = _randomPersonFactory.Create(i);

            }
        }
    }

    public static class DateHelpers
    {
        public static DateTime RandomDate(int minYear, int maxYear, int? seed = null)
        {
            var random = seed.HasValue ? new Random(seed.Value) : new Random();
            var monthOfBirth = new DateTime(random.Next(1950, 2006), random.Next(1, 12), 1);
            var dayOfBirth = random.Next(1, monthOfBirth.AddMonths(1).AddDays(-1).Day);
            return new DateTime(monthOfBirth.Year, monthOfBirth.Month, dayOfBirth);
        }
    }

    public interface IRandomPersonFactory
    {
        Person Create(int seed);
    }

    public class SimpleRandomRandomPersonFactory : IRandomPersonFactory
    {
        public Person Create(int seed)
        {
            return new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "First",
                LastName = "Last",
                DateOfBirth = DateHelpers.RandomDate(1935, 2005, seed),
                ProfilePictureUrl = "",
            };
        }
    }
}
