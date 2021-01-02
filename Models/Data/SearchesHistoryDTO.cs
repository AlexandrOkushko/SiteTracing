using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SiteTracing.Models.Data
{
    [Table("tblSearchHistory")]
    public class SearchesHistoryDTO
    {
        [Key]
        public int Id { get; set; }
        public string WebsiteAddress { get; set; }
    }
}