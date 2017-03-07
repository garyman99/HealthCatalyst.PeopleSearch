using System.Web.Mvc;
using PeopleSearch.Web.Models;

namespace PeopleSearch.Web.Controllers.MVC
{
    public class PersonSearchController : Controller
    {
        // GET: PersonSearch
        public ActionResult Index(string searchText = null, bool byInterest =false)
        {
            ViewBag.SearchText = searchText;
            ViewBag.ByInterest = byInterest;
            return View();
        }
    }
}
