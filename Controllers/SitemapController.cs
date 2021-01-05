using SiteTracing.Models.Data;
using SiteTracing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

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
        public ActionResult Create(SearchVM model)
        {
            

            //using (Db db = new Db())
            //{
            //    SearchDTO dto = new SearchDTO
            //    {
            //        WebsiteAddress = model.WebsiteAddress.ToLower()
            //    };

            //    db.Searches.Add(dto);
            //    db.SaveChanges();

            //    //#region Fill tblTraceDetails

            //    //int numberOfSearch = db.SearchesHistory.ToArray().Select(x => x.Id).Last();
            //    //Dictionary<IPAddress, ushort> pairs = GetTraceRoute(model.WebsiteAddress);

            //    //foreach (var pair in pairs)
            //    //{
            //    //    TraceDetailsDTO detailsDTO = new TraceDetailsDTO()
            //    //    {
            //    //        SearchId = numberOfSearch,
            //    //        Ip = pair.Key.ToString(),
            //    //        Ping = pair.Value
            //    //    };

            //    //    db.TraceDetails.Add(detailsDTO);
            //    //}

            //    //db.SaveChanges();

            //    //#endregion
            //}



            //return RedirectToAction("Index");



            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                    // Checks whether the input is correct url
                    Uri baseURI = new Uri(model.WebsiteAddress); // Сделать принудительную очистку 

                List<string> addressList = GetAllSiteFromSitemap(model.WebsiteAddress);

                return RedirectToAction("Index");
            }
            catch
            {
                // Write an exception
                return RedirectToAction("Create");
            }
        }

        private List<string> GetAllSiteFromSitemap(string address)
        {
            address += "sitemap.xml";

            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            
            string sitemapString = wc.DownloadString(address);

            //Create a new XML document and load the downloaded string as XML
            XmlDocument urldoc = new XmlDocument();
            urldoc.LoadXml(sitemapString);
            
            //Create an list of XML nodes from the url nodes in the sitemap
            XmlNodeList xmlSitemapList = urldoc.GetElementsByTagName("url");

            List<string> addressList = new List<string>();
            foreach (XmlNode node in xmlSitemapList)
            {
                if (node["loc"] != null)
                {
                    addressList.Add(node["loc"].InnerText);
                }
            }

            return addressList;
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
