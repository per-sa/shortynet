using System.ComponentModel.DataAnnotations;

namespace shortynet.Models
{
    public class Shortener
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime LastSeenDate { get; set; }
        public int RedirectCount { get; set; }
        public string Url { get; set; }
        public string Shortcode { get; set; }
    }
}