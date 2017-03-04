using System;
using Ninject;
using PeopleSearch.Seeder.Ioc.Exceptions;

namespace PeopleSearch.Seeder.Ioc
{
    public class SeederContainer
    {
        private readonly ISeedCoordinator _seedCoordinator;

        public SeederContainer(string[] args)
        {
            // note:  If I were you, I would wonder why I chose not to create the ninject kernal and get my 
            //        coorderinator from the main method.  In my experience allowing access to a container in your main method
            //        can easily lead to abuse of the container (ex: method/property injecting it in to dependencies
            //        and utilizing it as a service locator deep down in to your code).  I like to define
            //        a distinct interface around the kernel so that I can avoid any unintended anti-patterns
            //        by future developers.
            try
            {
                var kernel = new StandardKernel();
                kernel.Load(new SeederModule());
                _seedCoordinator = kernel.Get<ISeedCoordinator>();
            }
            catch (Exception exception)
            {
                throw new IocInitializationException(exception);
            }
        }

        public ISeedCoordinator GetSeedCoordinator()
        {
            return _seedCoordinator;
        }
    }
}
