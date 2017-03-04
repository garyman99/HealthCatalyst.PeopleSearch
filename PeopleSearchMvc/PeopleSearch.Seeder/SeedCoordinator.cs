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
        int Process();
    }

    public class SeedCoordinator : ISeedCoordinator
    {
        private readonly IList<ISeeder> _seeders;
        private readonly ILog _log;

        public SeedCoordinator(ILog log, IList<ISeeder> seeders)
        {
            _log = log;
            _seeders = seeders;
           
        }

        public int Process()
        {
            var exitCode = 0;

            _log.Info("Seeder is starting");
            if (_seeders.Any() == false)
            {
                _log.Warn("\tThere are no seeders registered!");
                exitCode = -1;
            }
            try
            {
                // start seeders
                var cancellationTokenSource = new CancellationTokenSource();
                var seedingTasks = new List<Task>();
                foreach (var seeder in _seeders)
                {
                    seedingTasks.Add(seeder.StartSeeding(cancellationTokenSource.Token));
                }

                // setup a message for when they're done
                Task.WhenAll(seedingTasks).ContinueWith(T =>
                {
                    var seedingCompleteMessage = cancellationTokenSource.IsCancellationRequested
                        ? "\tSeeders were successfully cancelled"
                        : "\tAll seeders completed without cancellation";
                    _log.Info(seedingCompleteMessage);
                }, cancellationTokenSource.Token);

                // wait for the user to cancel
                _log.Info("\tSeeding has started. Press any key to cancel seeding...");
                Console.ReadKey(false);
                cancellationTokenSource.Cancel();
                _log.Info("\tCancellation has been requested...");
                Task.WaitAll(seedingTasks.ToArray());
            }
            catch (Exception unhandledException)
            {
                Console.WriteLine($"Unhandled Excpetion in Seeder.  Details: \r\n{unhandledException}");
                exitCode = -1;
            }

            _log.Info("Seeder Finished");
            _log.Info("Press any key to exit");
            Console.ReadKey(false);
            return exitCode;
        }
    }
}
