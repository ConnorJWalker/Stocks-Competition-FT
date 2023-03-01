using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using StocksCompetition.Server.Models;

namespace StocksCompetition.Server.Entities;

public class ServerDbContext : IdentityDbContext<ApplicationUser>
{

}