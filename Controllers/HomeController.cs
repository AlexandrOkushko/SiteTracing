using SiteTracing.Models.Data;
using SiteTracing.Models.ViewModels.SearchesHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteTracing.Controllers
{
    public class HomeController : Controller
    {
        // GET: Index
        [HttpGet]
        public ActionResult Index()
        {
            List<SearchHistoryVM> searchHistoryVMList;
            using (Db db = new Db())
            {
                searchHistoryVMList = db.SearchesHistory.ToArray().OrderByDescending(x => x.Id).Select(x => new SearchHistoryVM(x)).ToList();
            }
            return View(searchHistoryVMList);
        }

        // GET: SearchQuery
        public ActionResult SearchQuery()
        {
            return View();
        }

        // POST: SearchQuery
        [HttpPost]
        public ActionResult SearchQuery(SearchHistoryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                SearchesHistoryDTO dto = new SearchesHistoryDTO();
                dto.WebsiteAddress = model.WebsiteAddress.ToLower();

                db.SearchesHistory.Add(dto);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details()
        {
            return View();
        }
    }
}