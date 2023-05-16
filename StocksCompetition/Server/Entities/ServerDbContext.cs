using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StocksCompetition.Server.Entities;

public class ServerDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(b =>
        {
            b.ToTable("users");
            b.HasKey(u => u.Id);
            b.HasMany<RefreshToken>(u => u.RefreshTokens)
                .WithOne(t => t.User);

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

        builder.Entity<RefreshToken>(b =>
        {
            b.Property(t => t.Token).HasColumnName("RefreshToken");
            b.HasKey(t => t.Token);
            
            b.HasOne<ApplicationUser>(t => t.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(t => t.UserId);
        });
    }
}