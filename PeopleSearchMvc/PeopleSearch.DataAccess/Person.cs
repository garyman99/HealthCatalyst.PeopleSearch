using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeopleSearch.DataAccess
{
    public class Person
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<PersonInterest> Interests { get; set; }
        public virtual ICollection<PersonImage> Images { get; set; }
    }
}