using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SiteTracing.Models.Data
{
    [Table("tblTraceDetails")]
    public class TraceDetailsDTO
    {
        [Key]
        public int Id { get; set; }
        public int SearchId { get; set; }
        public string Ip { get; set; }
        public int Ping { get; set; }

        [ForeignKey("CategoryId")]
        public virtual SearchesHistoryDTO SearchesHistory { get; set; }
    }
}