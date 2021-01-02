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
    }
}