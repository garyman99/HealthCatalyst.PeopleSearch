using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PeopleSearch.Seeder.Publishers.Api
{
    public class PeopleSearchApiPublisher : IPublisher
    {
        private readonly string _apiUri;

        public PeopleSearchApiPublisher(string apiUri)
        {
            _apiUri = apiUri;
        }
        
        public async Task Publish<T>(T person)
        {
            using (var client = new HttpClient())
            {
                await client.PostAsJsonAsync(_apiUri, person);
            }
        }

        public async Task Publish<T>(T person, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                await client.PostAsJsonAsync(_apiUri, person, cancellationToken);
            }
        }
    }
}