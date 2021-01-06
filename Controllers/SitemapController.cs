using SiteTracing.Models.Data;
using SiteTracing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                    // Checks whether the input is correct url
                    Uri baseURI = new Uri(model.WebsiteAddress); // Сделать принудительную очистку 

                List<string> addressList = GetAllSiteFromSitemap(model.WebsiteAddress);

                if (addressList.Count == 0)
                    throw new Exception();

                List<ushort> pingList = GetResponseTime(addressList);

                using (Db db = new Db())
                {
                    SearchDTO dto = new SearchDTO()
                    {
                        WebsiteAddress = model.WebsiteAddress
                    };

                    db.Searches.Add(dto);
                    db.SaveChanges();

                    int numberOfSearch = db.Searches.ToArray().Select(x => x.Id).Last();

                    for (int i = 0; i < addressList.Count; ++i)
                    {
                        SearchDetailsDTO detailsDTO = new SearchDetailsDTO()
                        {
                            SearchId = numberOfSearch,
                            Site = addressList[i],
                            Ping = pingList[i]
                        };

                        db.SearchDetails.Add(detailsDTO);
                    }

                    db.SaveChanges();
                }

                TempData["SM"] = "You have added a new search query!";

                return RedirectToAction("Index");
            }
            catch (UriFormatException)
            {
                TempData["DM"] = "The address is wrong, follow the advice!";
                return View();
            }
            catch (Exception)
            {
                TempData["DM"] = "There are no links in sitemap.xml or the site has no sitemap.xml!";
                return View();
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
                    addressList.Add(node["loc"].InnerText);
            }

            return addressList;
        }

        private List<ushort> GetResponseTime(List<string> addressList)
        {
            List<ushort> pings = new List<ushort>();

            foreach (string addres in addressList)
                pings.Add(GetPing(addres));

            return pings;
        }

        private ushort GetPing(string address)
        {
            ushort ping;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);

            Stopwatch timer = new Stopwatch();

            timer.Start();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            timer.Stop();

            ping = (ushort)timer.ElapsedMilliseconds;

            return ping;
        }

        // GET: Sitemap/Details/5
        public ActionResult Details(int id)
        {
            List<SearchDetailsVM> searchDetailsVMList;
            using (Db db = new Db())
            {
                searchDetailsVMList = db.SearchDetails.ToArray().Where(x => x.SearchId == id).OrderByDescending(x => x.Ping).Select(x => new SearchDetailsVM(x)).ToList();
            }
            TempData["Id"] = id;
            return View(searchDetailsVMList);
        }

        public ActionResult LoadData()
        {
            int id = int.Parse(TempData["Id"].ToString());

            List<SearchDetailsVM> data;
            using (Db db = new Db())
            {
                data = db.SearchDetails.ToArray().Where(x => x.SearchId == id).OrderByDescending(x => x.Ping).Select(x => new SearchDetailsVM(x)).ToList();
            }

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataForChart()
        {
            int id = int.Parse(TempData["Id"].ToString());
            TempData["Id"] = id;

            List<SearchDetailsVM> searchDetailsVMList;
            using (Db db = new Db())
            {
                searchDetailsVMList = db.SearchDetails.ToArray().Where(x => x.SearchId == id).OrderByDescending(x => x.Ping).Select(x => new SearchDetailsVM(x)).ToList();
            }

            var query = searchDetailsVMList.ToArray().Select(x => new { site = x.Site, ping = x.Ping }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        // POST: Sitemap/Delete/5
        public ActionResult Delete(int id)
        {
            using (Db db = new Db())
            {
                SearchDTO dto = db.Searches.Find(id);
                db.Searches.Remove(dto);
                db.SaveChanges();

                int traceDetailsCount = db.TraceDetails.ToArray().Where(x => x.SearchId == id).Count();
                for (int i = 0; i < traceDetailsCount; i++)
                {
                    SearchDetailsDTO detailsDTO = db.SearchDetails.FirstOrDefault(x => x.SearchId == id);
                    db.SearchDetails.Remove(detailsDTO);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
