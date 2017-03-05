using System;
using Ninject;
using Ninject.Modules;
using PeopleSearch.Seeder.Ioc.Exceptions;

namespace PeopleSearch.Seeder.Ioc
{
    public class SeederContainer
    {
        private readonly ISeedCoordinator _seedCoordinator;

        public SeederContainer(params NinjectModule[] ninjectModules)
        {
            // note:  I like limiting the usage of the container to only
            //        the root of the application.  otherwise you could end up
            //        with less experienced devs passing the kernel all over the
            //        place and implementing the service locator anti-pattern.
            try
            {
                var kernel = new StandardKernel();
                kernel.Load(new SeederModule());
                kernel.Load(ninjectModules);
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
