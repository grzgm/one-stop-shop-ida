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

        // GET: api/LunchTodayItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LunchTodayItem>>> GetLunchToday()
        {
          if (_context.LunchToday == null)
          {
              return NotFound();
          }
            return await _context.LunchToday.ToListAsync();
        }

        // GET: api/LunchTodayItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LunchTodayItem>> GetLunchTodayItem(string id)
        {
          if (_context.LunchToday == null)
          {
              return NotFound();
          }
            var lunchTodayItem = await _context.LunchToday.FindAsync(id);

            if (lunchTodayItem == null)
            {
                return NotFound();
            }

            return lunchTodayItem;
        }

        // PUT: api/LunchTodayItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLunchTodayItem(string id, LunchTodayItem lunchTodayItem)
        {
            if (id != lunchTodayItem.MicrosoftId)
            {
                return BadRequest();
            }

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

        // POST: api/LunchTodayItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LunchTodayItem>> PostLunchTodayItem(LunchTodayItem lunchTodayItem)
        {
          if (_context.LunchToday == null)
          {
              return Problem("Entity set 'DatabaseContext.LunchToday'  is null.");
          }
            _context.LunchToday.Add(lunchTodayItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LunchTodayItemExists(lunchTodayItem.MicrosoftId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLunchTodayItem", new { id = lunchTodayItem.MicrosoftId }, lunchTodayItem);
        }

        // DELETE: api/LunchTodayItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLunchTodayItem(string id)
        {
            if (_context.LunchToday == null)
            {
                return NotFound();
            }
            var lunchTodayItem = await _context.LunchToday.FindAsync(id);
            if (lunchTodayItem == null)
            {
                return NotFound();
            }

            _context.LunchToday.Remove(lunchTodayItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LunchTodayItemExists(string id)
        {
            return (_context.LunchToday?.Any(e => e.MicrosoftId == id)).GetValueOrDefault();
        }
    }
}
