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

        // POST: Home/SearchQuery
        [HttpPost]
        public ActionResult SearchQuery(SearchHistoryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                SearchesHistoryDTO dto = new SearchesHistoryDTO
                {
                    WebsiteAddress = model.WebsiteAddress.ToLower()
                };

                db.SearchesHistory.Add(dto);
                db.SaveChanges();

                #region Fill tblTraceDetails

                int numberOfSearch = db.SearchesHistory.ToArray().Select(x => x.Id).Last();
                Dictionary<IPAddress, ushort> pairs = GetTraceRoute(model.WebsiteAddress);

                foreach (var pair in pairs)
                {
                    TraceDetailsDTO detailsDTO = new TraceDetailsDTO()
                    {
                        SearchId = numberOfSearch,
                        Ip = pair.Key.ToString(),
                        Ping = pair.Value
                    };

                    db.TraceDetails.Add(detailsDTO);
                }

                db.SaveChanges();

                #endregion
            }

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

        // GET: Home/TracertRoute/id
        [HttpGet]
        public ActionResult TracertRoute(int id)
        {
            List<TraceDetailsVM> traceDetailsVMList;
            using (Db db = new Db())
            {
                traceDetailsVMList = db.TraceDetails.ToArray().Where(x => x.SearchId == id).Select(x => new TraceDetailsVM(x)).ToList();
            }

            return View(traceDetailsVMList);
        }


        public ActionResult Delete(int id)
        {
            using (Db db = new Db())
            {
                SearchesHistoryDTO dto = db.SearchesHistory.Find(id);
                db.SearchesHistory.Remove(dto);
                db.SaveChanges();

                int traceDetailsCount = db.TraceDetails.ToArray().Where(x => x.SearchId == id).Count();
                for (int i = 0; i < traceDetailsCount; i++)
                {
                    TraceDetailsDTO detailsDTO = db.TraceDetails.FirstOrDefault(x => x.SearchId == id);
                    db.TraceDetails.Remove(detailsDTO);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult GetDataForGraph(int id)
        {
            List<TraceDetailsVM> traceDetailsVMList;
            using (Db db = new Db())
            {
                traceDetailsVMList = db.TraceDetails.ToArray().Where(x => x.SearchId == id).Select(x => new TraceDetailsVM(x)).ToList();
            }

            var query = traceDetailsVMList.ToArray().Select(x => new { ip = x.Ip.ToString(), ping = x.Ping }).ToList();
            List<IpPing> ipPingList = traceDetailsVMList.ToArray().Select(x => new IpPing(x.SearchId.ToString(), x.Ping)).ToList();

            ViewBag.QUERY = query;

            var ips = traceDetailsVMList.ToArray().Select(x => x.SearchId);
            var pings = traceDetailsVMList.ToArray().Select(x => x.Ping).ToList();

            ViewBag.IPS = ips;
            ViewBag.PINGS = pings;

            return Json(ipPingList, JsonRequestBehavior.AllowGet);
        }

        public class IpPing
        {
            public IpPing()
            {

            }

            public IpPing(string s, int i)
            {
                Ip = s;
                Ping = i;
            }
            public string Ip { get; set; }
            public int Ping { get; set; }
        }

        public ActionResult GetDataPartial(int id)
        {
            List<TraceDetailsVM> traceDetailsVMList;
            using (Db db = new Db())
            {
                traceDetailsVMList = db.TraceDetails.ToArray().Where(x => x.SearchId == id).Select(x => new TraceDetailsVM(x)).ToList();
            }

            var query = traceDetailsVMList.ToArray().Select(x => new { ip = x.Ip.ToString(), ping = x.Ping }).ToList();
            List<IpPing> ipPingList = traceDetailsVMList.ToArray().Select(x => new IpPing(x.SearchId.ToString(), x.Ping)).ToList();

            ViewBag.QUERY = query;

            var ips = traceDetailsVMList.ToArray().Select(x => x.SearchId).ToList();
            var pings = traceDetailsVMList.ToArray().Select(x => x.Ping ).ToList();

            ViewBag.IPS = ips;
            ViewBag.PINGS = pings;





            return PartialView("_GetDataPartial");
        }
    }
}