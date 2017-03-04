using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PeopleSearch.Seeder.Ioc;

namespace PeopleSearch.Seeder
{
    class Program
    {
        static void Main(string[] args)
        {
            int exitCode = 0;
            Console.WriteLine("Seeder is starting");
            try
            {
                var seedContainer = new SeederContainer();
                var seeders = seedContainer.GetSeeders();

                if (seeders.Any() == false)
                {
                    Console.WriteLine("\tThere are no seeders registered!");
                }
                else
                {
                    var cancellationToken = new CancellationToken();
                    var seedingTasks = seeders.Select(consumer => consumer.StartSeeding(cancellationToken)).ToList();
                    
                    Task.WhenAll(seedingTasks).ContinueWith((delegate
                    {
                        var seedingCompleteMessage = cancellationToken.IsCancellationRequested
                            ? "\tSeeders were successfully cancelled"
                            : "\tAll seeders completed without cancellation";
                        Console.WriteLine(seedingCompleteMessage);
                    }), cancellationToken);

                    Console.WriteLine("\tSeeding has started. Press any key to cancel seeding...");
                    Console.ReadKey(false);
                    
                    Console.WriteLine("\tCancellation has been requested...");
                    Task.WaitAll(seedingTasks.ToArray());
                }
            }
            catch (Exception unhandledException)
            {
                Console.WriteLine($"Unhandled Excpetion in Seeder.  Details: \r\n{unhandledException}");
                exitCode = -1;
            }

            Console.Write("Seeder Finished");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey(false);
            Environment.Exit(exitCode);
        }
    }
}
