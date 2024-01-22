using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<IEnumerable<UserItem>> GetUserItems()
    {
        IsDbSetNull("Users");

        return await Users.ToListAsync();
    }

    public async Task<UserItem> GetUserItem(string microsoftId)
    {
        IsDbSetNull("Users");

        var userItem = await Users.FindAsync(microsoftId);

        if (userItem == null)
        {
            throw new KeyNotFoundException();
        }

        return userItem;
    }

    public async Task PutUserItem(UserItem userItem)
    {
        IsDbSetNull("Users");

        if (!UserItemExists(userItem.MicrosoftId))
        {
            throw new KeyNotFoundException();
        }

        Entry(userItem).State = EntityState.Modified;

        await SaveChangesAsync();
    }

    public async Task PostUserItem(UserItem userItem)
    {
        IsDbSetNull("Users");

        Users.Add(userItem);
        await SaveChangesAsync();
    }

    public async Task DeleteUserItem(string microsoftId)
    {
        IsDbSetNull("Users");

        var userItem = await Users.FindAsync(microsoftId);
        if (userItem == null)
        {
            throw new KeyNotFoundException();
        }

        Users.Remove(userItem);
        await SaveChangesAsync();
    }

    public bool UserItemExists(string microsoftId)
    {
        return (Users?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}