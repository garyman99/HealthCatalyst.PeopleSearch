using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PeopleSearch.Web.Startup))]
namespace PeopleSearch.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
