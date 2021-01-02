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
        [HttpGet]
        public ActionResult Index()
        {
            List<SearchHistoryVM> searchHistoryVMList;
            using (Db db = new Db())
            {
                searchHistoryVMList = db.SearchesHistory.ToArray().Select(x => new SearchHistoryVM(x)).ToList();
            }
            return View(searchHistoryVMList);
        }
    }
}