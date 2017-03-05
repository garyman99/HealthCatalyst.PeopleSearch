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

        // GET: api/Person
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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

            var dtoPerson = JsonConvert.DeserializeObject<BigCompany.Contracts.Person>(value);
            if (dtoPerson != null)
            {
                var dataAccessPerson = _context.People.FirstOrDefault(p => p.Id == dtoPerson.Id);
                if (dataAccessPerson == null)
                {
                    // create new entity
                    Post(value);
                }
                else
                {
                    // update the existing entity
                    dataAccessPerson.DateOfBirth = dtoPerson.DateOfBirth;
                    dataAccessPerson.FirstName = dtoPerson.FirstName;
                    dataAccessPerson.LastName = dtoPerson.LastName;
                    dataAccessPerson.Interests.Clear();
                    foreach (var dtoInterest in dtoPerson.Interests)
                    {
                        dataAccessPerson.Interests.Add(new PersonInterest { Interest = dtoInterest });
                    }
                    if (string.IsNullOrEmpty(dtoPerson.ProfilePictureUrl) == false)
                    {
                        //dataAccessPerson.Images.Add(new PersonImage {});
                    }
                    _context.SaveChanges();
                }
            }
           
        }

        // PUT: api/Person/
        public void Put([FromBody]string value)
        {
            // create a new person (or overwrite)--the result of this call should always be the same!
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
                if (string.IsNullOrEmpty(dtoPerson.ProfilePictureUrl) == false)
                {
                    //dataAccessPerson.Images.Add(new PersonImage {});
                }
                foreach (var dtoInterest in dtoPerson.Interests)
                {
                    dataAccessPerson.Interests.Add(new PersonInterest { Interest = dtoInterest });
                }
                _context.People.Add(dataAccessPerson);
            }
        }

        // DELETE: api/Person/5
        public void Delete(int id)
        {
        }
    }
}
