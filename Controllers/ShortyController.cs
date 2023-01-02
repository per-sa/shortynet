using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shortynet.Models;
using shortynet.Data;

namespace shortynet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShortyController : ControllerBase
    {
        private readonly ShortyDbContext dbContext;
        private string GenerateShortcode()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
            string shortcode = "";
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                shortcode += chars[random.Next(chars.Length)];
            }
            return shortcode;

        }

        public ShortyController(ShortyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        [HttpGet]
        public async Task<IActionResult> GetShorty()
        {
            return Ok(await dbContext.Shorteners.ToListAsync());
        }

        [HttpGet("{shortcode}")]
        public async Task<IActionResult> GetShorty(string shortcode)
        {
            var shorty = await dbContext.Shorteners.FirstOrDefaultAsync(s => s.Shortcode == shortcode);
            if (shorty == null)
            {
                return NotFound();
            }
            shorty.LastSeenDate = DateTime.Now;
            shorty.RedirectCount++;
            await dbContext.SaveChangesAsync();
            return Redirect(shorty.Url);

        }

        [HttpGet("{shortcode}/stats")]
        public async Task<IActionResult> GetShortyStats(string shortcode)
        {
            var shorty = await dbContext.Shorteners.FirstOrDefaultAsync(s => s.Shortcode == shortcode);
            if (shorty == null)
            {
                return NotFound();
            }

            if (shorty.RedirectCount == 0)
            {
                return Ok(new { startDate = shorty.StartDate, redirectCount = shorty.RedirectCount });
            }

            return Ok(shorty);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShorty(CreateShorty createShorty)
        {
            var shorty = new Shortener
            {
                StartDate = DateTime.Now,
                LastSeenDate = DateTime.Now,
                RedirectCount = 0,
                Url = createShorty.Url,
                Shortcode = createShorty.Shortcode
            };

            if (shorty.Shortcode == null || shorty.Shortcode == "")
            {
                shorty.Shortcode = GenerateShortcode();
            }

            if (await dbContext.Shorteners.AnyAsync(s => s.Shortcode == shorty.Shortcode))
            {
                return Conflict(new { error = $"An existing record with the shortcode '{shorty.Shortcode}' was already found." });
            }

            // check if the shortcode is a valid regex
            if (!System.Text.RegularExpressions.Regex.IsMatch(shorty.Shortcode, @"^[0-9a-zA-Z_]{4,}$"))
            {
                return UnprocessableEntity(new { error = "The shortcode fails to meet the following regexp: ^[0-9a-zA-Z_]{4,}$." });
            }



            dbContext.Shorteners.Add(shorty);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetShorty), new { shortcode = shorty.Shortcode }, shorty);
        }
    }
}
