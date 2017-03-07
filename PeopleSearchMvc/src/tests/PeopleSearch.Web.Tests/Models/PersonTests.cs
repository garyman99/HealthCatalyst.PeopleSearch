using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework.Internal;
using NUnit.Framework;
using PeopleSearch.Web.Models;

namespace PeopleSearch.Web.Tests.Models
{
    [TestFixture]
    public class PersonTests
    {
        [Test]
        [TestCase("Gary","Hibbard", "Gary Hibbard")]
        [TestCase("Bonno",null,"Bonno")]
        [TestCase("Cher", "", "Cher")]
        [TestCase(null, "Trump", "Trump")]
        public void NameTests(string first, string last, string expected)
        {
            // ARRANGE
            // ACT
            var person = new PersonModel(1,first,last,DateTime.MinValue, new string[] {});

            // ASSERT
            person.Name.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void DobTests_NoPaddingRequired()
        {
            // ARRANGE
            var dob = new DateTime(1995, 12, 25, 5, 35, 31);
            // ACT
            var person = new PersonModel(1, "Test", "Person", dob, new string[] { });

            // ASSERT
            person.DateOfBirth.ShouldBeEquivalentTo("12/25/1995");
        }

        [Test]
        public void DobTests_PaddingRequired()
        {
            // ARRANGE
            var dob = new DateTime(1995, 2, 5);
            // ACT
            var person = new PersonModel(1, "Test", "Person", dob, new string[] { });

            // ASSERT
            person.DateOfBirth.ShouldBeEquivalentTo("2/5/1995");
        }
    }
}
