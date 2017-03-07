using System.Collections.Generic;
using System.Linq;
using PeopleSearch.DataAccess.Entities.People;

namespace PeopleSearch.DataAccess.Repositories
{
    public interface IPersonRepository
    {
        void Delete(int id);
        void Add(BigCompany.Contracts.Person dtoPerson);
        IList<Person> FindByName(string searchValue);
        Person GetById(int id);
        IList<Person> FindByInterest(string snowboarding);
    }

    public class PersonRepository : IPersonRepository
    {
        private readonly PeopleSearchContext _context;

        public PersonRepository(PeopleSearchContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var person = _context.People.FirstOrDefault(p => p.Id == id);
            if (person != null)
            {
                _context.People.Remove(person);
                _context.SaveChanges();
            }
        }

        public void Add(BigCompany.Contracts.Person dtoPerson)
        {
            // get the existing or instantiate a new person
            Person dataAccessPerson = null;
            if (dtoPerson.Id != default(int))
            {
                dataAccessPerson = _context.People.FirstOrDefault(p => p.Id == dtoPerson.Id);
            }
            if (dataAccessPerson == null)
            {
                dataAccessPerson = new Person();
                _context.People.Add(dataAccessPerson);
            }

            // update the values on the person
            dataAccessPerson.DateOfBirth = dtoPerson.DateOfBirth;
            dataAccessPerson.FirstName = dtoPerson.FirstName;
            dataAccessPerson.LastName = dtoPerson.LastName;
            
            // todo: separate API calls for posting interests  ex: /api/person/1/interests/1
            if (dataAccessPerson.Interests == null)
            {
                dataAccessPerson.Interests = new List<PersonInterest>();
            } else if(dataAccessPerson.Interests.Any())
            {
                dataAccessPerson.Interests.Clear();
            }
            foreach (var dtoInterest in dtoPerson.Interests)
            {
                dataAccessPerson.Interests.Add(new PersonInterest { Interest = dtoInterest });
            }

            // todo: separate API calls for posting image(s)
            if (string.IsNullOrEmpty(dtoPerson.ImageBase64) == false)
            {
                dataAccessPerson.Image = new PersonImage {ImageBase64 = dtoPerson.ImageBase64};
            }

            _context.SaveChanges();
        }

        public IList<Person> FindByName(string searchString)
        {
            return _context.People.Where(p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString)).ToList();
        }

        public IList<Person> FindByInterest(string interestName)
        {
            return _context.Interests.Where(i => i.Interest == interestName).Select(i => i.Person).Distinct().ToList();
        }

        public Person GetById(int id)
        {
            return _context.People.FirstOrDefault(p => p.Id == id);   
        }
    }
}
