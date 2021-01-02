using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteTracing.Models.ViewModels.SearchesHistory
{
    public class SearchHistoryVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 8)]
        public string WebsiteAddress { get; set; }
    }
}