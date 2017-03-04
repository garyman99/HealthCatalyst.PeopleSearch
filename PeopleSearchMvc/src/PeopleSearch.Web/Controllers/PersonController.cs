using System.Collections.Generic;
using System.Web.Http;
using PeopleSearch.DataAccess;

namespace PeopleSearch.Web.Controllers
{
    public class PersonController : ApiController
    {
        private PeopleSearchContext _context;

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
            return _context.
        }

        // POST: api/Person
        public void Post([FromBody]string value)
        {

        }

        // PUT: api/Person/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE: api/Person/5
        public void Delete(int id)
        {
        }
    }
}
