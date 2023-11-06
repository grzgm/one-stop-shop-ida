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
    public class LunchRecurringItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public LunchRecurringItemsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/LunchRecurringItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LunchRecurringItem>>> GetLunchRecurring()
        {
          if (_context.LunchRecurring == null)
          {
              return NotFound();
          }
            return await _context.LunchRecurring.ToListAsync();
        }

        // GET: api/LunchRecurringItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LunchRecurringItem>> GetLunchRecurringItem(string id)
        {
          if (_context.LunchRecurring == null)
          {
              return NotFound();
          }
            var lunchRecurringItem = await _context.LunchRecurring.FindAsync(id);

            if (lunchRecurringItem == null)
            {
                return NotFound();
            }

            return lunchRecurringItem;
        }

        // PUT: api/LunchRecurringItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLunchRecurringItem(string id, LunchRecurringItem lunchRecurringItem)
        {
            if (id != lunchRecurringItem.MicrosoftId)
            {
                return BadRequest();
            }

            _context.Entry(lunchRecurringItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LunchRecurringItemExists(id))
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

        // POST: api/LunchRecurringItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LunchRecurringItem>> PostLunchRecurringItem(LunchRecurringItem lunchRecurringItem)
        {
          if (_context.LunchRecurring == null)
          {
              return Problem("Entity set 'DatabaseContext.LunchRecurring'  is null.");
          }
            _context.LunchRecurring.Add(lunchRecurringItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LunchRecurringItemExists(lunchRecurringItem.MicrosoftId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLunchRecurringItem", new { id = lunchRecurringItem.MicrosoftId }, lunchRecurringItem);
        }

        // DELETE: api/LunchRecurringItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLunchRecurringItem(string id)
        {
            if (_context.LunchRecurring == null)
            {
                return NotFound();
            }
            var lunchRecurringItem = await _context.LunchRecurring.FindAsync(id);
            if (lunchRecurringItem == null)
            {
                return NotFound();
            }

            _context.LunchRecurring.Remove(lunchRecurringItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LunchRecurringItemExists(string id)
        {
            return (_context.LunchRecurring?.Any(e => e.MicrosoftId == id)).GetValueOrDefault();
        }
    }
}
