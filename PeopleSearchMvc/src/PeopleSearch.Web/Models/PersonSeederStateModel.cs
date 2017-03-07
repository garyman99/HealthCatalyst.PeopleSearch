using System.Collections.Generic;
using PeopleSearch.Seeder;

namespace PeopleSearch.Web.Models
{
    public class PersonSeederStateModel
    {
        public PersonSeederStateModel(SeedingState seedingState)
        {
            // todo: do not like this
            NotStarted = seedingState == SeedingState.Uninitiated;
            Started = seedingState == SeedingState.Seeding;
            Stopping = seedingState == SeedingState.Cancelling;
            Stopped = seedingState == SeedingState.Cancelled
                      || seedingState == SeedingState.Completed
                      || seedingState == SeedingState.Errored
                      || seedingState == SeedingState.Uninitiated;
            Errored = seedingState == SeedingState.Errored;
        }

        public bool NotStarted { get; private set; }
        public bool Started { get; private set; }
        public bool Stopping { get; private set; }
        public bool Stopped { get; private set; }
        public bool Errored { get; private set; }
        public string ErrorMessage { get; set; }

        public List<string> LogMessages { get; private set; }

        public void AddLogs(List<string> logs)
        {
            LogMessages = logs;
        }
    }
}