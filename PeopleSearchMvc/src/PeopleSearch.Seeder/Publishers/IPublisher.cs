using System.Threading;
using System.Threading.Tasks;

namespace PeopleSearch.Seeder.Publishers
{
    public interface IPublisher
    {
        Task Publish<T>(T person);

        Task Publish<T>(T person, CancellationToken cancellationToken);
    }
}