using System;

namespace PeopleSearch.Seeder.Ioc.Exceptions
{
    public class IocInitializationException : Exception
    {
        private const string IocErrorMessage = "Error initializing IoC container. See inner exception for details";
        public IocInitializationException(Exception innerException) : base(IocErrorMessage, innerException)
        {
        }
    }
}