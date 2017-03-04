using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using BigCompany.Contracts;
using Common.Logging;
using PeopleSearch.Seeder.PersonFactories.Random;
using PeopleSearch.Seeder.Publishers;

namespace PeopleSearch.Seeder.Seeders.Random
{
    public class RandomPersonSeeder : RandomSeeder
    {
        private readonly IRandomPersonFactory _randomPersonFactory;
        private readonly IPublisher<Person> _publisher;


        public RandomPersonSeeder(TaskFactory taskFactory, ILog log, RandomSeederOptions options, IRandomPersonFactory randomPersonFactory, IPublisher<Person> publisher)
            : base(taskFactory, log, options)
        {
            _randomPersonFactory = randomPersonFactory;
            _publisher = publisher;
        }

        internal override async Task PublishRandom(int seedNumber, CancellationToken cancellationToken)
        {
            var person = _randomPersonFactory.Create(seedNumber);

            await _publisher.Publish(person, cancellationToken);
        }
    }
}