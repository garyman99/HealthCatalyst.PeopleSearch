using Common.Logging;
using Common.Logging.Simple;
using Ninject.Modules;

namespace PeopleSearch.Seeder.Console.Ioc
{
    public class ConsoleLoggerModule : NinjectModule
    {
        public override void Load()
        {
            var consoleLogger = new ConsoleOutLogger("Seeder", LogLevel.All, true, true, false, "yyyy/MM/dd hh:mm:ss");
            Bind<ILog>().ToConstant(consoleLogger);
        }
    }
}