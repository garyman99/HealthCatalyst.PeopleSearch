using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BigCompany.Contracts;

namespace PeopleSearch.Seeder.Publishers.Api
{
    public class PeopleSearchApiIntegrator : IPublisher<Person>
    {
        private readonly string _apiUri;

        public PeopleSearchApiIntegrator(string apiUri)
        {
            _apiUri = apiUri;
        }
        
        public async Task<HttpResponseMessage> Publish(Person person)
        {
            using (var client = new HttpClient())
            {
                return await client.PutAsJsonAsync(_apiUri, person);
            }
        }

        public async Task<HttpResponseMessage> Publish(Person person, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                return await client.PutAsJsonAsync(_apiUri, person, cancellationToken);
            }
        }
    }
}