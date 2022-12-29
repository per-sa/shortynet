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

        public ShortyController(ShortyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetShorty()
        {
            return Ok(await dbContext.Shorteners.ToListAsync());
        }
    }
}











































