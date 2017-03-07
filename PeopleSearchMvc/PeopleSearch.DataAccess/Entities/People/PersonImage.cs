using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleSearch.DataAccess.Entities.People
{
    public class PersonImage
    {
        [Key, ForeignKey("Person")]
        public int PersonId { get; set; }

        [Required]
        public string ImageBase64 { get; set; }
        
        public Person Person { get; set; }
    }
}