using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers
{
    [Route("lunch/recurring")]
    [ApiController]
    public class LunchRecurringItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public LunchRecurringItemsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("get-registered-days")]
        public async Task<ActionResult<LunchRecurringItem>> GetRegisteredDays()
        {
            string microsoftId = HttpContext.Session.GetString("microsoftId");
            if (_context.LunchRecurring == null)
            {
                return NotFound();
            }
            var lunchRecurringItem = await _context.LunchRecurring.FindAsync(microsoftId);

            if (lunchRecurringItem == null)
            {
                return NotFound();
            }

            return lunchRecurringItem;
        }

        [HttpPut("update-registered-days")]
        public async Task<IActionResult> PutLunchRecurringItem([FromQuery] LunchRecurringItem lunchRecurringItem)
        {
            string microsoftId = HttpContext.Session.GetString("microsoftId");
            lunchRecurringItem.MicrosoftId = microsoftId;

            _context.Entry(lunchRecurringItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LunchRecurringItemExists(microsoftId))
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

        [HttpPost("create-lunch-recurring")]
        public async Task PostLunchRecurringItem(string microsoftId)
        {
            LunchRecurringItem lunchRecurringItem = new();
            lunchRecurringItem.MicrosoftId = microsoftId;
            lunchRecurringItem.Monday = false;
            lunchRecurringItem.Tuesday = false;
            lunchRecurringItem.Wednesday = false;
            lunchRecurringItem.Thursday = false;
            lunchRecurringItem.Friday = false;

            if (_context.LunchRecurring == null)
            {
                throw new DbUpdateException();
            }
            _context.LunchRecurring.Add(lunchRecurringItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        private bool LunchRecurringItemExists(string id)
        {
            return (_context.LunchRecurring?.Any(e => e.MicrosoftId == id)).GetValueOrDefault();
        }
    }
}
