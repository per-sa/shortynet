using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace shortynet.Models
{
    [Index(nameof(Shortcode), IsUnique = true)]
    public class CreateShorty
    {
        [Required]
        public string Url { get; set; }
        // Only Unique Shortcodes
        [StringLength(6, MinimumLength = 6)]
        public string Shortcode { get; set; }
    }
}