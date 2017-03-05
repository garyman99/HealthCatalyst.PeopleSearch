using System.Threading.Tasks;
using Common.Logging;
using Common.Logging.Simple;
using Ninject.Modules;
using PeopleSearch.Seeder.Factories.Random;
using PeopleSearch.Seeder.Factories.Random.Simple;
using PeopleSearch.Seeder.Publishers;
using PeopleSearch.Seeder.Publishers.Api;
using PeopleSearch.Seeder.Publishers.Log;
using PeopleSearch.Seeder.Seeders;
using PeopleSearch.Seeder.Seeders.Random;

namespace PeopleSearch.Seeder.Ioc
{
    public class SeederModule : NinjectModule
    {
        public override void Load()
        {
            Bind<TaskFactory>().ToSelf();

            Bind<IRandomPersonFactory>().To<SimpleRandomPersonFactory>();
            Bind<IPublisher>().To<LogPublisher>();
            Bind<IPublisher>().To<PeopleSearchApiPublisher>()
                .WithConstructorArgument("apiUri", "http://localhost:63949/api/Person");

            Bind<RandomSeederOptions>().ToConstant(new RandomSeederOptions(100, 10000));
            Bind<ISeeder>().To<RandomPersonSeeder>();
            //TaskFactory taskFactory, ILog log, RandomSeederOptions options,
            //IRandomPersonFactory randomPersonFactory, IEnumerable<IPublisher> publisherses

            Bind<ISeedCoordinator>().To<SeedCoordinator>();
        }
    }
}