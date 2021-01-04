using SiteTracing.Models.Data;
using SiteTracing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteTracing.Controllers
{
    public class SitemapController : Controller
    {
        // GET: Sitemap
        public ActionResult Index()
        {
            List<SearchVM> searchVMList;
            using (Db db = new Db())
                searchVMList = db.Searches.ToArray().OrderByDescending(x => x.Id).Select(x => new SearchVM(x)).ToList();

            if (searchVMList.Count == 0)
            {
                ViewBag.Message = "Your search history is empty.";
                return View();
            }

            return View(searchVMList);
        }
        
        // GET: Sitemap/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sitemap/Create
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

        // GET: Sitemap/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sitemap/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sitemap/Delete/5
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
