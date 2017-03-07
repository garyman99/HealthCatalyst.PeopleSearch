using System;

namespace PeopleSearch.Web.Models
{
    public class PersonModel
    {
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly DateTime _dateOfBirth;

        public PersonModel(int id, string firstName, string lastName, DateTime dob, string[] interests, string base64Image = null)
        {
            Id = id;
            _firstName =firstName;
            _lastName = lastName;
            _dateOfBirth = dob;
            Interests = interests;
            Image = base64Image;
        }

        public string Name
        {
            get { return $"{_firstName} {_lastName}".Trim(); }
        }

        public int Id { get; private set; }

        public string DateOfBirth
        {
            get { return _dateOfBirth.ToString("d"); }
        }

        public string Image { get; private set; }

        public string[] Interests { get; private set; }
    }
}