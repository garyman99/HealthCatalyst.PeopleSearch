using System;
using FluentAssertions;
using NUnit.Framework;
using PeopleSearch.Seeder.Ioc;

namespace PeopleSearch.Seeder.Tests
{
    [TestFixture]
    public class IocContainerTests
    {
        [Test]
        public void Resolving()
        {
            // ARRANGE
            var seederContainer = new SeederContainer(new string[] {});

            // ACT
            var seeders = seederContainer.GetSeedCoordinator();

            // ASSERT
            seeders.Should().NotBeNull();
        }
    }
}
