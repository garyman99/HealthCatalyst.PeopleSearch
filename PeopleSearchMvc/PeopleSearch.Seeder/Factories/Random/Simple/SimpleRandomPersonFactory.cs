using System;
using BigCompany.Contracts;

namespace PeopleSearch.Seeder.PersonFactories.Random.Simple
{
    public class SimpleRandomPersonFactory : IRandomPersonFactory
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