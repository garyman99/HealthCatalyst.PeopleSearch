using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PeopleSearch.Seeder.Publishers
{
    public interface IPublisher<in T>
    {
        Task<HttpResponseMessage> Publish(T person);

        Task<HttpResponseMessage> Publish(T person, CancellationToken cancellationToken);
    }
}