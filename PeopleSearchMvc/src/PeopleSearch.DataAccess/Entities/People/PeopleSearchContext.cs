using System.Data.Entity;

namespace PeopleSearch.DataAccess.Entities.People
{
    public class PeopleSearchContext : DbContext, IPeopleSearchContext
    {
        public virtual IDbSet<Person> People { get; set; }
        public virtual IDbSet<PersonImage> PersonImages { get; set; }
        public virtual IDbSet<PersonInterest> Interests { get; set; }
    }
}