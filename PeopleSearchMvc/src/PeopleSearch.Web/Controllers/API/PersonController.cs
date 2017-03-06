using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using PeopleSearch.DataAccess;

namespace PeopleSearch.Web.Controllers.API
{
    public class PersonController : ApiController
    {
        private readonly PeopleSearchContext _context;

        public PersonController(PeopleSearchContext context)
        {
            _context = context;
        }

        // GET: api/Person/search
        public string Get(string searchString)
        {
            var matches = _context.People.Where(p => p.FirstName.Contains(searchString)
                                                     || p.LastName.Contains(searchString)).ToList();
            
            return JsonConvert.SerializeObject(matches);
        }

        // GET: api/Person/5
        public string Get(int id)
        {
            var person = _context.People.FirstOrDefault(p => p.Id == id);

            return JsonConvert.SerializeObject(person);
        }

        // POST: api/Person
        public void Post([FromBody]string value)
        {
            // modify or update an existing person

            if (value == null)
            {
                return;
            }

            var dtoPerson = JsonConvert.DeserializeObject<BigCompany.Contracts.Person>(value);
            if (dtoPerson != null)
            {
                var dataAccessPerson = _context.People.FirstOrDefault(p => p.Id == dtoPerson.Id);
                if (dataAccessPerson == null)
                {
                    // create new entity
                    Put(value);
                }
                else
                {
                    // update the existing entity
                    dataAccessPerson.DateOfBirth = dtoPerson.DateOfBirth;
                    dataAccessPerson.FirstName = dtoPerson.FirstName;
                    dataAccessPerson.LastName = dtoPerson.LastName;
                    dataAccessPerson.Interests.Clear();
                    // todo: separate API calls for posting image(s)
                    if (string.IsNullOrEmpty(dtoPerson.ImageBase64) == false)
                    {
                        dataAccessPerson.Images.Add(new PersonImage { ImageBase64 = dtoPerson.ImageBase64 });
                    }
                    // todo: separate API calls for posting interests  ex: /api/person/1/interests/1
                    foreach (var dtoInterest in dtoPerson.Interests)
                    {
                        dataAccessPerson.Interests.Add(new PersonInterest { Interest = dtoInterest });
                    }
                    _context.SaveChanges();
                }
            }
        }

        // PUT: api/Person/
        public void Put([FromBody]string value)
        {
            // create a new person (or overwrite)--the result of this call should always be the same!

            if (value == null)
            {
                return;
            }

            var dtoPerson = JsonConvert.DeserializeObject<BigCompany.Contracts.Person>(value);
            if (dtoPerson != null)
            {
                // verify we don't have an existing data access object
                Delete(dtoPerson.Id);

                // create our new data access object
                var dataAccessPerson = new Person
                {
                    DateOfBirth = dtoPerson.DateOfBirth,
                    FirstName = dtoPerson.FirstName,
                    LastName = dtoPerson.LastName,
                    Interests = new List<PersonInterest>()
                };
                if (string.IsNullOrEmpty(dtoPerson.ImageBase64) == false)
                {
                    dataAccessPerson.Images.Add(new PersonImage {ImageBase64 = dtoPerson.ImageBase64});
                }
                foreach (var dtoInterest in dtoPerson.Interests)
                {
                    dataAccessPerson.Interests.Add(new PersonInterest { Interest = dtoInterest });
                }
                _context.People.Add(dataAccessPerson);
                _context.SaveChanges();
            }
        }

        // DELETE: api/Person/5
        public void Delete(int id)
        {
            var person = _context.People.FirstOrDefault(p => p.Id == id);
            if (person != null)
            {
                _context.People.Remove(person);
                _context.SaveChanges();
            }
        }
    }
}
