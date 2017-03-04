using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using PeopleSearch.Seeder.Seeders.Random;

namespace PeopleSearch.Seeder.Seeders
{
    public abstract class RandomSeeder : ISeeder
    {
        private readonly TaskFactory _taskFactory;
        private readonly ILog _log;
        private readonly RandomSeederOptions _options;

        protected RandomSeeder(TaskFactory taskFactory, ILog log, RandomSeederOptions options)
        {
            _taskFactory = taskFactory;
            _log = log;
            _options = options;
        }

        public Task StartSeeding(CancellationToken cancellationToken)
        {
            // define a long-running action that will be used to run while the process isnt cancelled and while we 
            // haven't exceeded the maximum seed amount
            Action seedCoordinator = () =>
            {
                var count = 0;
                while (cancellationToken.IsCancellationRequested == false && count < _options.MaxSeedAmount)
                {
                    // create a list of sub-tasks to wait for.  include, by default, the call to the PublishRandom() 
                    // method on the derived class.  if we are in the delay phase of our processing, include a delay 
                    // task as well.
                    var subTasks = new List<Task> {PublishRandom(count, cancellationToken)};
                    if (count > _options.InitialSeedAmount)
                    {
                        subTasks.Add(Task.Delay(_options.Delay.Milliseconds, cancellationToken));
                    }
                    Task.WaitAll(subTasks.ToArray());

                    count += 1;
                    if (count%_options.LogEverySeeds == 0)
                    {
                        _log.Info($"\t\t{count} people seeded");
                    }
                }
            };

            // start the long-running process within a task and give the task back to the consumer to await
            return _taskFactory.StartNew(seedCoordinator, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        internal abstract Task PublishRandom(int seedNumber, CancellationToken cancellationToken);
    }
}