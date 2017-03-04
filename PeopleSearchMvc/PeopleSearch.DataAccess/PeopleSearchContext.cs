using System.Data.Entity;

namespace PeopleSearch.DataAccess
{
    public class PeopleSearchContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<PersonImage> PersonImages { get; set; }
        public DbSet<PersonInterest> Interests { get; set; }
    }
}