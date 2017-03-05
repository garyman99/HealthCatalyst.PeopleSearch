using System.Threading;
using System.Threading.Tasks;

namespace PeopleSearch.Seeder.Seeders
{
    public interface ISeeder
    {
        Task StartSeeding(CancellationToken cancellationToken);
    }
}