using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BigCompany.Contracts;

namespace PeopleSearch.Seeder.Publishers.Api
{
    public class PeopleSearchApiPublisher : IPublisher<Person>
    {
        private readonly string _apiUri;

        public PeopleSearchApiPublisher(string apiUri)
        {
            _apiUri = apiUri;
        }
        
        public async Task Publish(Person person)
        {
            using (var client = new HttpClient())
            {
                await client.PutAsJsonAsync(_apiUri, person);
            }
        }

        public async Task Publish(Person person, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                await client.PutAsJsonAsync(_apiUri, person, cancellationToken);
            }
        }
    }
}