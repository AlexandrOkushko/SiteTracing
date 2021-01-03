using SiteTracing.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SiteTracing.Models.ViewModels
{
    public class TraceDetailsVM
    {
        public TraceDetailsVM()
        {

        }

        public TraceDetailsVM(TraceDetailsDTO row)
        {
            Id = row.Id;
            SearchId = row.SearchId;
            Ip = row.Ip;
            Ping = row.Ping;
        }

        public int Id { get; set; }
        public int SearchId { get; set; }
        
        [DisplayName("Ip addres")]
        public string Ip { get; set; }
        public int Ping { get; set; }
    }
}