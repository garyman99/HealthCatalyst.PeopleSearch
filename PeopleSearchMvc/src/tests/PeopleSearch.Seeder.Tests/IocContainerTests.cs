using FluentAssertions;
using NUnit.Framework;
using PeopleSearch.Seeder.Console.Ioc;
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
            var seederContainer = new SeederContainer(new ConsoleLoggerModule());

            // ACT
            var seeders = seederContainer.GetSeedCoordinator();

            // ASSERT
            seeders.Should().NotBeNull();
        }
    }
}
