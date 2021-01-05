using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteTracing.Models.Data
{
    [Table("tblSearchDetails")]
    public class SearchDetailsDTO
    {
        [Key]
        public int Id { get; set; }
        public int SearchId { get; set; }
        public string Site { get; set; }
        public int Ping { get; set; }

        [ForeignKey("SearchId")]
        public virtual SearchDTO Search { get; set; }
    }
}