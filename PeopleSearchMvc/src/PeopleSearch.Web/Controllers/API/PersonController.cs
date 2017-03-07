using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using Newtonsoft.Json;
using PeopleSearch.DataAccess.Repositories;
using PeopleSearch.Web.Models;

namespace PeopleSearch.Web.Controllers.API
{
    public class PersonController : ApiController
    {
        private readonly IPersonRepository _repository;

        public PersonController(IPersonRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Person/search
        public string Get(string searchString, bool byInterest = false)
        {
            var matches = byInterest
                ? _repository.FindByInterest(searchString)
                : _repository.FindByName(searchString);

            var models = new List<PersonModel>();
            foreach (var match in matches)
            {
                var model = new PersonModel(match.Id,
                    match.FirstName,
                    match.LastName,
                    match.DateOfBirth,
                    match.Interests?.Select(i => i.Interest).ToArray(),
                    match.Image?.ImageBase64);
                models.Add(model);
            }

            // introduce some fake delay
            Thread.Sleep(3000);

            var jsonResult = JsonConvert.SerializeObject(models.ToArray());
            return jsonResult;
        }

        // GET: api/Person/5
        public string Get(int id)
        {
            PersonModel model = null;
            var match = _repository.GetById(id);
            if (match != null)
            {
                model = new PersonModel(match.Id,
                    match.FirstName,
                    match.LastName,
                    match.DateOfBirth,
                    match.Interests?.Select(i => i.Interest).ToArray(),
                    match.Image?.ImageBase64);
            }

            return JsonConvert.SerializeObject(model);
        }

        // POST: api/Person/1
        public void Post([FromBody] BigCompany.Contracts.Person value)
        {
            // modify or update an existing person
            if (value == null)
            {
                return;
            }
             _repository.Add(value);
        }

        // DELETE: api/Person/5
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
