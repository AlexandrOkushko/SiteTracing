using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteTracing.Models.Data
{
    [Table("tblSearch")]
    public class SearchDTO
    {
        [Key]
        public int Id { get; set; }

        public string WebsiteAddress { get; set; }
    }
}