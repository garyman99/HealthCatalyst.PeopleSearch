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

        public async Task Publish<T>(T person)
        {
            var jsonPerson = JsonConvert.SerializeObject(person);
            await _taskFactory.StartNew(()=>_log.Debug(jsonPerson));
        }

        public async Task Publish<T>(T person, CancellationToken cancellationToken)
        {
            var jsonPerson = JsonConvert.SerializeObject(person);
            await _taskFactory.StartNew(() => _log.Debug(jsonPerson), cancellationToken);
        }
    }
}
