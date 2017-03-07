using BigCompany.Contracts;

namespace PeopleSearch.Seeder.Factories.Random
{
    public interface IRandomPersonFactory
    {
        Person Create(int seed);
    }
}