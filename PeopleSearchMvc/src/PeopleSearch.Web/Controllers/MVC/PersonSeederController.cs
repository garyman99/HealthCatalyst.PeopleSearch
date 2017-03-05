using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using PeopleSearch.Seeder;

namespace PeopleSearch.Web.Controllers
{
    public class PersonSeederController : Controller
    {
        private readonly ISeedCoordinator _seedCoordinator;

        public PersonSeederController(ISeedCoordinator seedCoordinator)
        {
            _seedCoordinator = seedCoordinator;
        }
        
        // GET: PersonSearch
        public ActionResult State()
        {
            return View();
        }
    }
}
