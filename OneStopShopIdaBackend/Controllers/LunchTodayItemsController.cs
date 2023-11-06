using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LunchTodayItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public LunchTodayItemsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("is-registered")]
        public async Task<ActionResult<bool>> GetLunchTodayIsRegistered()
        {
            if (_context.LunchToday == null)
            {
                return NotFound();
            }
            var lunchTodayItem = await _context.LunchToday.FindAsync(HttpContext.Session.GetString("microsoftId"));

            if (lunchTodayItem == null)
            {
                return NotFound();
            }

            return lunchTodayItem.IsRegistered;
        }

        [HttpPut("update-is-registered")]
        public async Task<IActionResult> PutLunchTodayRegister([FromQuery] string id, [FromQuery] bool IsRegistered)
        {
            var lunchTodayItem = await _context.LunchToday.FindAsync(id);
            lunchTodayItem.IsRegistered = IsRegistered;
            _context.Entry(lunchTodayItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LunchTodayItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("create-is-registered/{microsoftId}")]
        public async Task PostLunchTodayItem(string microsoftId)
        {
            LunchTodayItem lunchTodayItem = new();
            lunchTodayItem.MicrosoftId = microsoftId;
            lunchTodayItem.IsRegistered = false;

            _context.LunchToday.Add(lunchTodayItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException();
            }
        }

        private bool LunchTodayItemExists(string id)
        {
            return (_context.LunchToday?.Any(e => e.MicrosoftId == id)).GetValueOrDefault();
        }
    }
}
