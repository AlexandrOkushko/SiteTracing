using SiteTracing.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace SiteTracing.Models.ViewModels
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