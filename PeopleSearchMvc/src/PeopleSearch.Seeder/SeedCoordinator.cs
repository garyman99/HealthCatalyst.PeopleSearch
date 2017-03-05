using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using PeopleSearch.Seeder.Seeders;

namespace PeopleSearch.Seeder
{
    public interface ISeedCoordinator
    {
        Task StartProcessing();
        Task Cancel();
    }

    public class SeedCoordinator : ISeedCoordinator
    {
        private readonly TaskFactory _taskFactory;
        private readonly List<ISeeder> _seeders;
        private readonly ILog _log;

        private readonly CancellationTokenSource _cancellationTokenSource;

        public SeedCoordinator(TaskFactory taskFactory, ILog log, IEnumerable<ISeeder> seeders)
        {
            _taskFactory = taskFactory;
            _log = log;
            _seeders = seeders.ToList();

            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartProcessing()
        {
            await _taskFactory.StartNew(() =>
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

                    // setup a message for when they're done
                    Action<Task> action = T =>
                    {
                        var seedingCompleteMessage = _cancellationTokenSource.IsCancellationRequested
                            ? "\tSeeders were successfully cancelled"
                            : "\tAll seeders completed without cancellation";
                        _log.Info(seedingCompleteMessage);
                    };
                    Task.WhenAll(seedingTasks).ContinueWith(action, _cancellationTokenSource.Token);
                }
                catch (Exception unhandledException)
                {
                    _log.Error($"Unhandled Excpetion in Seeder.  Details: \r\n{unhandledException}");
                }
            });
        }

        public async Task Cancel()
        {
            await _taskFactory.StartNew(() => _cancellationTokenSource.Cancel());
        }
    }
}
