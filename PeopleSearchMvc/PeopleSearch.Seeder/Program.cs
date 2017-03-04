using System;
using PeopleSearch.Seeder.Ioc;

namespace PeopleSearch.Seeder
{
    class Program
    {
        static void Main(string[] args)
        {
            var seedContainer = new SeederContainer(args);
            var seedCoordinator = seedContainer.GetSeedCoordinator();
            var resultCode = seedCoordinator.Process();

            Environment.Exit(resultCode);
        }
    }
}
