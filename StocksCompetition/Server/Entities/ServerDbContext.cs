using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StocksCompetition.Server.Models;

namespace StocksCompetition.Server.Entities;

public class ServerDbContext : IdentityDbContext<ApplicationUser>
{
    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(b =>
        {
            b.ToTable("users");
            b.HasKey(u => u.Id);

            b.Property(u => u.DisplayName).HasMaxLength(64);
            b.Property(u => u.DiscordUsername).HasMaxLength(64);
            b.Property(u => u.DisplayColour).HasMaxLength(8);

            b.Ignore(u => u.Email);
            b.Ignore(u => u.EmailConfirmed);
            b.Ignore(u => u.NormalizedEmail);
            b.Ignore(u => u.UserName);
            b.Ignore(u => u.NormalizedUserName);
            b.Ignore(u => u.PhoneNumber);
            b.Ignore(u => u.PhoneNumberConfirmed);
            b.Ignore(u => u.TwoFactorEnabled);
        });
    }
}