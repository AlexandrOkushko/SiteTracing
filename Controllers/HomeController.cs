using SiteTracing.Models.Data;
using SiteTracing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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

            // instead of var there will be a connection to the database
            var tmp = GetTraceRoute(model.WebsiteAddress);

            return RedirectToAction("Index");
        }

        #region Trace
        public static Dictionary<IPAddress, ushort> GetTraceRoute(string hostname)
        {
            Dictionary<IPAddress, ushort> pairs = new Dictionary<IPAddress, ushort>();

            const int timeout = 1000; // 1 second
            const int maxTTL = 30;
            const int bufferSize = 32;

            byte[] buffer = new byte[bufferSize];
            new Random().NextBytes(buffer);

            using (Ping pinger = new Ping())
            {
                Stopwatch pingReplyTime = new Stopwatch();

                for (int ttl = 1; ttl <= maxTTL; ttl++)
                {
                    PingOptions options = new PingOptions(ttl, true);
                    pingReplyTime.Start();
                    PingReply reply = pinger.Send(hostname, timeout, buffer, options);
                    pingReplyTime.Stop();

                    // we've found a route at this ttl
                    if (reply.Status == IPStatus.Success || reply.Status == IPStatus.TtlExpired)
                    {
                        pairs.Add(reply.Address, (ushort)pingReplyTime.ElapsedMilliseconds);
                        pingReplyTime.Reset();
                    }

                    // if we reach a status other than expired or timed out, we're done searching or there has been an error
                    if (reply.Status != IPStatus.TtlExpired && reply.Status != IPStatus.TimedOut)
                        break;
                }
            }
            return pairs;
        }
        #endregion

        [HttpGet]
        public ActionResult Details(int id)
        {

            return View();
        }
    }
}