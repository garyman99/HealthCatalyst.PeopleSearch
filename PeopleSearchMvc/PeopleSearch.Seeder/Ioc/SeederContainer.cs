using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject;
using PeopleSearch.Seeder.Ioc.Exceptions;
using PeopleSearch.Seeder.Seeders;

namespace PeopleSearch.Seeder.Ioc
{
    public class SeederContainer
    {
        private readonly IList<ISeeder> _seeders;

        public SeederContainer()
        {
            // note:  If I were you, I would wonder why I chose not to create the ninject kernal and get my 
            //        seeders from there.  In my experience allowing access to a container in your main method
            //        can easily lead to abuse of the container (ex: method/property injecting it in to dependencies
            //        and utilizing it as a service locator deep down in to your code).  I like to define
            //        a distinct interface around the kernel so that I can avoid any unintended anti-patterns
            //        by future developers.
            try
            {
                var kernel = new StandardKernel();
                kernel.Load(Assembly.GetExecutingAssembly());
                _seeders = kernel.GetAll<ISeeder>().ToList();
            }
            catch (Exception exception)
            {
                throw new IocInitializationException(exception);
            }
        }

        public IList<ISeeder> GetSeeders()
        {
            return _seeders;
        }
    }
}
