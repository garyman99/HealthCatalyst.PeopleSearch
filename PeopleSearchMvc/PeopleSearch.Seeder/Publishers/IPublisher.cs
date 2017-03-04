using System.Threading;
using System.Threading.Tasks;

namespace PeopleSearch.Seeder.Publishers
{
    public interface IPublisher<in T>
    {
        Task Publish(T person);

        Task Publish(T person, CancellationToken cancellationToken);
    }
}