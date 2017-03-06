using Ninject.Modules;
using PeopleSearch.DataAccess;
using PeopleSearch.Web.Controllers.API;
using PeopleSearch.Web.Controllers.MVC;

namespace PeopleSearch.Web.Ioc
{
    public class PeopleSearchModule : NinjectModule
    {
        public override void Load()
        {
            // Data Access
            Bind<PeopleSearchContext>().ToSelf();

            // API
            Bind<PersonController>().ToSelf();

            // MVC 
            Bind<PersonSearchController>().ToSelf();
        }
    }
}