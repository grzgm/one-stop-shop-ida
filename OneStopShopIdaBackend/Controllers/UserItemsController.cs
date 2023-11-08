using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers
{
    public class UserItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UserItemsController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<ActionResult<IEnumerable<UserItem>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }
        public async Task<ActionResult<UserItem>> GetUserItem(string id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var userItem = await _context.Users.FindAsync(id);

            if (userItem == null)
            {
                return NotFound();
            }

            return userItem;
        }
        public async Task<bool> GetIsUserInDatabase(string id)
        {
            if (_context.Users == null)
            {
                return false;
            }
            var userItem = await _context.Users.FindAsync(id);

            if (userItem == null)
            {
                return false;
            }

            return true;
        }
        public async Task<IActionResult> PutUserItem(string id, UserItem userItem)
        {
            if (id != userItem.MicrosoftId)
            {
                return BadRequest();
            }

            _context.Entry(userItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserItemExists(id))
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
        public async Task<ActionResult<UserItem>> PostUserItem(UserItem userItem)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'DatabaseContext.Users'  is null.");
          }
            _context.Users.Add(userItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserItemExists(userItem.MicrosoftId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserItem", new { id = userItem.MicrosoftId }, userItem);
        }
        public async Task<IActionResult> DeleteUserItem(string id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var userItem = await _context.Users.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserItemExists(string id)
        {
            return (_context.Users?.Any(e => e.MicrosoftId == id)).GetValueOrDefault();
        }
    }
}
