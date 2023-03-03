using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StocksCompetition.Server.Models;

namespace StocksCompetition.Server.Entities;

public static class UserManagerExtensions
{
    public static async Task<ApplicationUser?> FindByDiscordUsernameAsync(this UserManager<ApplicationUser> userManager, string discordUsername)
    {
        return await userManager.Users.SingleOrDefaultAsync(u => u.DiscordUsername == discordUsername);
    }
}