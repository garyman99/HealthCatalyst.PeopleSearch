using BigCompany.Contracts;

namespace PeopleSearch.Seeder.Factories.Random.Simple
{
    public class SimpleRandomPersonFactory : IRandomPersonFactory
    {
        public Person Create(int seed)
        {
            var person = new Person
            {
                FirstName = "First",
                LastName = "Last",
                DateOfBirth = DateHelpers.RandomDate(1935, 2005, seed),
                ImageBase64 = "",
                Interests = StaticData.RandomInterests(seed),
            };

            return person;
        }
    }
}