using SiteTracing.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteTracing.Models.ViewModels.SearchesHistory
{
    public class SearchHistoryVM
    {
        public SearchHistoryVM()
        {

        }

        public SearchHistoryVM(SearchesHistoryDTO row)
        {
            Id = row.Id;
            WebsiteAddress = row.WebsiteAddress;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 7)]
        [Display(Name = "Website address")]
        public string WebsiteAddress { get; set; }
    }
}