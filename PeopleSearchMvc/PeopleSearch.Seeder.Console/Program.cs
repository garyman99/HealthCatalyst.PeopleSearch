using System.Threading.Tasks;
using PeopleSearch.Seeder.Console.Ioc;
using PeopleSearch.Seeder.Ioc;

namespace PeopleSearch.Seeder.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // instantiate coordinator and start seeding
            var seedContainer = new SeederContainer(new ConsoleLoggerModule());
            var seedCoordinator = seedContainer.GetSeedCoordinator();
            var coordinatorTask = seedCoordinator.StartProcessing();
            
            // wait for the user to cancel
            System.Console.WriteLine("\tSeeding has started. Press any key to cancel seeding...");
            System.Console.ReadKey(false);
            var cancelTask = seedCoordinator.Cancel();

            // wait for coordinators or cancellation to finish
            Task.WaitAny(coordinatorTask, cancelTask);
            System.Console.WriteLine("Seeder Finished");
            System.Console.WriteLine("Press any key to exit");
            System.Console.ReadKey(false);
        }
    }
}
