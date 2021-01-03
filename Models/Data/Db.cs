using SiteTracing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SiteTracing.Models.Data
{
    public class Db : DbContext
    {
        public DbSet<SearchesHistoryDTO> SearchesHistory { get; set; }

        public DbSet<TraceDetailsDTO> TraceDetails { get; set; }

        public System.Data.Entity.DbSet<SiteTracing.Models.ViewModels.TraceDetailsVM> TraceDetailsVMs { get; set; }
    }
}