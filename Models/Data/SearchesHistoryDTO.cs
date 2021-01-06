using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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