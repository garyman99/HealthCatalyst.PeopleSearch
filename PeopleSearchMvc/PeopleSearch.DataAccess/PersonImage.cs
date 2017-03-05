using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleSearch.DataAccess
{
    public class PersonImage
    {
        [Key]
        public int PersonId { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
    }
}