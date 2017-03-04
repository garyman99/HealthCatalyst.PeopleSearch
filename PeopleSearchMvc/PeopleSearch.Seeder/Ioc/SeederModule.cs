using System.Threading.Tasks;
using BigCompany.Contracts;
using Common.Logging;
using Common.Logging.Simple;
using Ninject.Modules;
using PeopleSearch.Seeder.PersonFactories.Random;
using PeopleSearch.Seeder.PersonFactories.Random.Simple;
using PeopleSearch.Seeder.Publishers;
using PeopleSearch.Seeder.Publishers.Log;
using PeopleSearch.Seeder.Seeders;
using PeopleSearch.Seeder.Seeders.Random;

namespace PeopleSearch.Seeder.Ioc
{
    public class SeederModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILog>().ToConstant(new ConsoleOutLogger("Seeder", LogLevel.All, true, true, false, "yyyy/MM/dd hh:mm:ss"));
            Bind<TaskFactory>().ToSelf();

            Bind<IRandomPersonFactory>().To<SimpleRandomPersonFactory>();
            Bind<IPublisher<Person>>().To<LogPublisher>();
            //Bind<IPublisher<Person>>().To<PeopleSearchApiPublisher>().WithConstructorArgument("apiName", "");

            Bind<RandomSeederOptions>().ToConstant(new RandomSeederOptions(100, 10000));
            Bind<ISeeder>().To<RandomPersonSeeder>();

            Bind<ISeedCoordinator>().To<SeedCoordinator>();
        }
    }
}