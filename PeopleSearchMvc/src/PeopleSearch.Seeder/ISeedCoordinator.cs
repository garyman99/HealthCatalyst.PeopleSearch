using System.Threading.Tasks;

namespace PeopleSearch.Seeder
{
    public interface ISeedCoordinator
    {
        SeedingState State { get; }

        Task StartProcessing();
        Task Cancel();
    }
}