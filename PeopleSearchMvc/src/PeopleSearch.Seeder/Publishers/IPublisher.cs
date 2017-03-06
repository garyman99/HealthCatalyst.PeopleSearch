using System.Threading;
using System.Threading.Tasks;

namespace PeopleSearch.Seeder.Publishers
{
    public interface IPublisher
    {
        Task Publish<T>(T input);

        Task Publish<T>(T input, CancellationToken cancellationToken);
    }
}