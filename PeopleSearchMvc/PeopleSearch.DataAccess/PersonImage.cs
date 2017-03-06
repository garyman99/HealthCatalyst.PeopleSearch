using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleSearch.DataAccess
{
    public class PersonImage
    {
        [Key]
        public int PersonId { get; set; }

        [Required]
        public string ImageBase64 { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
    }
}