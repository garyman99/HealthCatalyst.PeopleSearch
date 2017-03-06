using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using PeopleSearch.Seeder.Seeders;

namespace PeopleSearch.Seeder
{
    public class SeedCoordinator : ISeedCoordinator
    {
        private readonly TaskFactory _taskFactory;
        private readonly List<ISeeder> _seeders;
        private readonly ILog _log;

        private CancellationTokenSource _cancellationTokenSource;
        private Task _processingTask;

        public SeedCoordinator(TaskFactory taskFactory, ILog log, IEnumerable<ISeeder> seeders)
        {
            _taskFactory = taskFactory;
            _log = log;
            _seeders = seeders.ToList();

            State = SeedingState.Uninitiated;
        }

        public SeedingState State { get; private set; }

        public async Task StartProcessing()
        {
            State = SeedingState.Seeding;
            _cancellationTokenSource = new CancellationTokenSource();
            _processingTask = _taskFactory.StartNew(() =>
            {
                _log.Info("Seed coordinator is starting");
                if (_seeders.Any() == false)
                {
                    _log.Warn("\tThere are no seeders registered!");
                }
                try
                {
                    // start seeders
                    var seedingTasks = new List<Task>();
                    foreach (var seeder in _seeders)
                    {
                        seedingTasks.Add(seeder.StartSeeding(_cancellationTokenSource.Token));
                    }

                    Task.WaitAll(seedingTasks.ToArray());
                }
                catch (Exception unhandledException)
                {
                    _log.Error($"Unhandled Excpetion in Seeder.  Details: \r\n{unhandledException}");
                }
            });

            await _processingTask;
        }

        public async Task Cancel()
        {
            if (_cancellationTokenSource == null)
            {
                throw new Exception("Unable to cancel seeding because the cancellation token is null");
            }
            _cancellationTokenSource.Cancel();
            
            await _processingTask.ContinueWith((T) =>
            {
                _log.Info("\tSeeders were successfully cancelled");
                State = SeedingState.Cancelled;
            });
        }
    }
}
