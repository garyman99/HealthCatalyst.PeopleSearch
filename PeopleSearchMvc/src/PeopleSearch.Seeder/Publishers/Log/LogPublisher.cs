using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using Newtonsoft.Json;

namespace PeopleSearch.Seeder.Publishers.Log
{
    public class LogPublisher : IPublisher
    {
        private readonly ILog _log;
        private readonly TaskFactory _taskFactory;

        public LogPublisher(ILog log, TaskFactory taskFactory)
        {
            _log = log;
            _taskFactory = taskFactory;
        }

        public async Task Publish<T>(T input)
        {
            await _taskFactory.StartNew(()=>_log.Debug(input));
        }

        public async Task Publish<T>(T input, CancellationToken cancellationToken)
        {
            await _taskFactory.StartNew(() => _log.Debug(input), cancellationToken);
        }
    }
}
