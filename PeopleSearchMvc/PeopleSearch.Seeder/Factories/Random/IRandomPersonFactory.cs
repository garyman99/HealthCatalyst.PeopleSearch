using BigCompany.Contracts;

namespace PeopleSearch.Seeder.PersonFactories.Random
{
    public interface IRandomPersonFactory
    {
        Person Create(int seed);
    }
}