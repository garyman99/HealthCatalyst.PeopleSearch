using System;
using System.Collections.Generic;
using System.Linq;
using FakeDbSet;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PeopleSearch.DataAccess;
using PeopleSearch.DataAccess.Entities.People;
using PeopleSearch.DataAccess.Repositories;
using PeopleSearch.Web.Models;

namespace PeopleSearch.Web.Tests.Controllers.API
{
    [TestFixture]
    public class PersonControllerTests
    {
        private readonly Mock<PeopleSearchContext> _mockContext;
        private readonly InMemoryDbSet<Person> _persons;
        private readonly InMemoryDbSet<PersonInterest> _interests;
        private readonly InMemoryDbSet<PersonImage> _images;
        private readonly PersonRepository _personRepository;

        public PersonControllerTests()
        {
            _persons = new InMemoryDbSet<Person>();
            _interests = new InMemoryDbSet<PersonInterest>();
            _images = new InMemoryDbSet<PersonImage>();

            for (int i = 1; i < 31; i++)
            {
                var newPerson = new Person
                {
                    Id = i,
                    DateOfBirth = new DateTime(1990, 1, i),
                    FirstName = $"First{i}",
                    LastName = $"last{i}",
                    Image = new PersonImage { PersonId = i, ImageBase64 = i.ToString() },
                    Interests = new List<PersonInterest>(),
                };
                for (int j = 1; j < i / 2; j++)
                {
                    var newInterst = new PersonInterest {Id = i ^ j, Interest = $"Interest{i}_{j}", PersonId = i, Person = newPerson};
                    newPerson.Interests.Add(newInterst);
                    _interests.Add(newInterst);
                }
                if (i == 10 || i == 11)
                {
                    var snowboardingInterest = new PersonInterest { Id = 1000+ i, Interest = "Snowboarding", PersonId = i, Person = newPerson };
                    newPerson.Interests.Add(snowboardingInterest);
                    _interests.Add(snowboardingInterest);
                }

                _images.Add(newPerson.Image);
                _persons.Add(newPerson);
            }

            _mockContext = new Mock<PeopleSearchContext>();
            _mockContext.Setup(x => x.People).Returns(_persons);
            _mockContext.Setup(x => x.Interests).Returns(_interests);
            _mockContext.Setup(x => x.PersonImages).Returns(_images);

            _personRepository = new PersonRepository(_mockContext.Object);
        }

        [Test]
        public void GetTests()
        {
            // ARRANGE 
            var controller = new PeopleSearch.Web.Controllers.API.PersonController(_personRepository);

            // ACT
            var result = controller.Get(5);

            // ASSERT
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("First5");
            // todo: the linkage to the inerests/image in the mock data context isn't working
        }

        [Test]
        public void SearchTests()
        {
            // ARRANGE 
            var controller = new PeopleSearch.Web.Controllers.API.PersonController(_personRepository);

            // ACT
            var results = controller.Get("ast2");
            var deserialized = JsonConvert.DeserializeObject<PersonModel[]>(results);

            // ASSERT
            deserialized.Should().NotBeNull();
            deserialized.Length.Should().Be(11);
        }

        [Test]
        public void SearchTests_byInterest()
        {
            // ARRANGE 
            var controller = new PeopleSearch.Web.Controllers.API.PersonController(_personRepository);

            // ACT
            var results = controller.Get("Snowboarding", true);
            var deserialized = JsonConvert.DeserializeObject<PersonModel[]>(results);

            // ASSERT
            deserialized.Should().NotBeNull();
            deserialized.Length.Should().Be(2);
        }

        [Test]
        public void PostTests()
        {
            // ARRANGE 
            var controller = new PeopleSearch.Web.Controllers.API.PersonController(_personRepository);
            var dtoPerson = new BigCompany.Contracts.Person()
            {
                DateOfBirth = new DateTime(1985,5,22),
                FirstName = "UniqueString",
                LastName = "Last",
                ImageBase64 = "test",
                Interests = new[] {"Snowboarding", "Skydiving", "Faberge Eggs"}
            };

            // ACT
            controller.Post(dtoPerson);

            // ASSERT
            _persons.ToList().Any(x => x.FirstName == "UniqueString").Should().BeTrue();
            _mockContext.Verify(x=>x.SaveChanges());
        }


        [Test]
        public void DeleteTests()
        {
            // ARRANGE 
            var controller = new PeopleSearch.Web.Controllers.API.PersonController(_personRepository);

            // ACT
            controller.Delete(1);

            // ASSERT
            _persons.ToList().Any(p => p.Id == 1).Should().BeFalse();
            _mockContext.Verify(x => x.SaveChanges());
        }
    }
}
