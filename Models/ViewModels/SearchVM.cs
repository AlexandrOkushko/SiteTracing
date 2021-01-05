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
        [StringLength(150, MinimumLength = 16)] // MinimumLength = 16. Because the shortest domain is 16 characters. "http://www.g.cn/".
        [Display(Name = "Website address")]
        public string WebsiteAddress { get; set; }
    }
}