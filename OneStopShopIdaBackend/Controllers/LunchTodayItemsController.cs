using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers
{
    [Route("lunch/today")]
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
        
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateAllLunchTodayItems(bool isRegistered)
        {
            var lunchTodayItems = await _context.LunchToday.ToListAsync();

            foreach (var lunchTodayItem in lunchTodayItems)
            {
                lunchTodayItem.IsRegistered = isRegistered;
                _context.Entry(lunchTodayItem).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle exceptions if necessary
                return StatusCode(500, "Internal Server Error");
            }
        }

        private bool LunchTodayItemExists(string id)
        {
            return (_context.LunchToday?.Any(e => e.MicrosoftId == id)).GetValueOrDefault();
        }
    }
}
