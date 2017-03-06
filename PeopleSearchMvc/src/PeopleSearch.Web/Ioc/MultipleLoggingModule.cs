using System.Collections.Generic;
using Common.Logging;
using Common.Logging.Simple;
using Ninject.Modules;

namespace PeopleSearch.Web.Ioc
{
    public class MultipleLoggingModule : NinjectModule
    {
        public override void Load()
        {
            var capturingLoggerAdapter = new CapturingLoggerFactoryAdapter();
            Bind<CapturingLoggerFactoryAdapter>().ToConstant(capturingLoggerAdapter);

            // create the loggers
            var consoleLogger = new ConsoleOutLogger("Seeder", LogLevel.All, true, true, false, "yyyy/MM/dd hh:mm:ss");
            var capturingLogger = capturingLoggerAdapter.GetLogger("Seeder");
            var allLoggers = new List<ILog> {consoleLogger, capturingLogger };
            var multipleLogger = new Common.Logging.MultipleLogger.MultiLogger(allLoggers);

            Bind<ILog>().ToConstant(multipleLogger);
        }
    }
}