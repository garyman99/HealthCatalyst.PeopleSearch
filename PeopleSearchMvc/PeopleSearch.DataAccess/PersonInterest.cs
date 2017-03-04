using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleSearch.DataAccess
{
    public class PersonInterest
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        [Required]
        public string Interest { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
    }
}
