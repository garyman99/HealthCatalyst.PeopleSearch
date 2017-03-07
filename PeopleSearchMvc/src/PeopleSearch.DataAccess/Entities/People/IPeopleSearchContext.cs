using System.Data.Entity;

namespace PeopleSearch.DataAccess.Entities.People
{
    public interface IPeopleSearchContext 
    {
        IDbSet<Person> People { get; set; }
        IDbSet<PersonImage> PersonImages { get; set; }
        IDbSet<PersonInterest> Interests { get; set; }
    }
}