using Ninject.Modules;
using PeopleSearch.Seeder.PersonFactories.Random;
using PeopleSearch.Seeder.PersonFactories.Random.Simple;
using PeopleSearch.Seeder.Seeders;
using PeopleSearch.Seeder.Seeders.Random;

namespace PeopleSearch.Seeder.Ioc
{
    public class SeederModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRandomPersonFactory>().To<SimpleRandomPersonFactory>();


            Bind<ISeeder>().To<RandomPersonSeeder>();
        }
    }
}