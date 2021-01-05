using SiteTracing.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteTracing.Models.ViewModels
{
    public class SearchDetailsVM
    {
        public SearchDetailsVM()
        {

        }

        public SearchDetailsVM(SearchDetailsDTO row)
        {
            SearchId = row.SearchId;
            Site = row.Site;
            Ping = row.Ping;
        }

        public int SearchId { get; set; }
        public string Site { get; set; }
        public int Ping { get; set; }

        public IEnumerable<SelectListItem> SearchIds { get; set; }
    }
}