using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace shortynet.Models
{
    [Index(nameof(Shortcode), IsUnique = true)]
    public class CreateShorty
    {
        [Required]
        public string Url { get; set; }
        // Only Unique Shortcodes
        [AllowNull]
        [RegularExpression(@"^[0-9a-zA-Z_]{4,}$")]
        public string Shortcode { get; set; }
    }
}