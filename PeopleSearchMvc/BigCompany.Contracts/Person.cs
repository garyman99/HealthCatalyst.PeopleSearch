using System;

namespace BigCompany.Contracts
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImageBase64 { get; set; }
        public string[] Interests { get; set; }

        public override string ToString()
        {
            var interests = Interests == null || Interests.Length == 1
                ? "None"
                : string.Join(",", Interests);
            return $"Id: {Id}; Name: {FirstName} {LastName};" +
                   $" Dob: {DateOfBirth.ToString("d")};" +
                   $" Interests: {interests}";
        }
    }
}
