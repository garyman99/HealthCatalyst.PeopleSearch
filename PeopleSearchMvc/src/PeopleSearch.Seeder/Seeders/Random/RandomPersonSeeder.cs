using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using PeopleSearch.Seeder.Factories.Random;
using PeopleSearch.Seeder.Publishers;

namespace PeopleSearch.Seeder.Seeders.Random
{
    public class RandomPersonSeeder : ISeeder
    {

        private readonly TaskFactory _taskFactory;
        private readonly ILog _log;
        private readonly RandomSeederOptions _options;

        private readonly IRandomPersonFactory _randomPersonFactory;
        private readonly List<IPublisher> _publishers;


        public RandomPersonSeeder(TaskFactory taskFactory, ILog log, RandomSeederOptions options,
            IRandomPersonFactory randomPersonFactory, IEnumerable<IPublisher> publishers)
        {
            _taskFactory = taskFactory;
            _log = log;
            _options = options;

            _randomPersonFactory = randomPersonFactory;
            _publishers = publishers.ToList();
        }

        public Task StartSeeding(CancellationToken cancellationToken)
        {
            // define a long-running action that will be used to run while the process isnt cancelled and while we 
            // haven't exceeded the maximum seed amount
            Action producerTask = async () =>
            {
                var count = 0;
                while (cancellationToken.IsCancellationRequested == false && count < _options.MaxSeedAmount)
                {
                    // publish random people
                    var random = _randomPersonFactory.Create(count);
                    var publishTasks = new List<Task>();
                    foreach (var publisher in _publishers)
                    {
                        publishTasks.Add(publisher.Publish(random, cancellationToken));
                    }
                    Task.WaitAll(publishTasks.ToArray());

                    // delay if we've exceeded the initial seed amount
                    if (count > _options.InitialSeedAmount)
                    {
                        var delay = (int)_options.Delay.TotalMilliseconds;
                        _log.Debug($"Delaying by {delay} milliseconds");
                        await Task.Delay(delay, cancellationToken).ContinueWith(T => { });
                    }

                    count += 1;
                    if (count % _options.LogEverySeeds == 0)
                    {
                        _log.Info($"\t\t{count} people seeded");
                    }
                }
            };

            // start the long-running process within a task and give the task back to the consumer to await
            return _taskFactory.StartNew(producerTask, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }
}