using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeopleSearch.Web.Controllers
{
    public class PersonSearchController : Controller
    {
        // GET: PersonSearch
        public ActionResult Index()
        {
            return View();
        }

        // GET: PersonSearch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PersonSearch/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonSearch/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonSearch/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PersonSearch/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonSearch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PersonSearch/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
