using SiteTracing.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace SiteTracing.Models.ViewModels
{
    public class SearchVM
    {
        public SearchVM()
        {

        }

        public SearchVM(SearchDTO row)
        {
            Id = row.Id;
            WebsiteAddress = row.WebsiteAddress;
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Website address")]
        public string WebsiteAddress { get; set; }
    }
}