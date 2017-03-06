using System;
using System.Linq;
using System.Web.Mvc;
using Common.Logging.Simple;
using PeopleSearch.Seeder;
using PeopleSearch.Web.Models;

namespace PeopleSearch.Web.Controllers
{
    public class PersonSeederController : Controller
    {
        private readonly ISeedCoordinator _seedCoordinator;
        private readonly CapturingLoggerFactoryAdapter _capturingLoggerAdapter;

        public PersonSeederController(ISeedCoordinator seedCoordinator,
            CapturingLoggerFactoryAdapter capturingLoggerAdapter)
        {
            _seedCoordinator = seedCoordinator;
            _capturingLoggerAdapter = capturingLoggerAdapter;
        }

        // GET: PersonSeeder
        public ActionResult Index()
        {
            var seedingState = _seedCoordinator.State;
            var state = new PersonSeederStateModel(seedingState);
          
            return View(state);
        }

        // POST: PersonSeeder/GetState
        [System.Web.Mvc.HttpPost]
        public ActionResult GetState()
        {
            var state = new PersonSeederStateModel(_seedCoordinator.State);

            // todo: an event-based solution to capturing these logs would be much better
            //       as you would be able to maintain, in user's session, a history of log events 
            //       that have occurred.  opening two windows causes some weird stuff with this now.
            var logs = _capturingLoggerAdapter.LoggerEvents.ToList().Select(e => e.RenderedMessage).ToList();
            _capturingLoggerAdapter.LoggerEvents.Clear();
            state.AddLogs(logs);

            return Json(state);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult StartSeeding()
        {
            PersonSeederStateModel state;
            try
            {
                _seedCoordinator.StartProcessing();
                state = new PersonSeederStateModel(_seedCoordinator.State);
            }
            catch (Exception ex)
            {
                state = new PersonSeederStateModel(_seedCoordinator.State)
                {
                    ErrorMessage = ex.ToString()
                };
            }
            return Json(state);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult StopSeeding()
        {
            PersonSeederStateModel state;
            try
            {
                _seedCoordinator.Cancel();
                state = new PersonSeederStateModel(_seedCoordinator.State);
            }
            catch (Exception ex)
            {
                state = new PersonSeederStateModel(_seedCoordinator.State)
                {
                    ErrorMessage = ex.ToString()
                };
            }
            return Json(state);
        }
    }
}
