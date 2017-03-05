using Common.Logging;
using Common.Logging.Simple;
using Ninject.Modules;

namespace PeopleSearch.Web.Ioc
{
    public class CommonLoggingLog4NetModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILog>().ToConstant(new ConsoleOutLogger("Seeder", LogLevel.All, true, true, false, "yyyy/MM/dd hh:mm:ss"));
        }
    }
}