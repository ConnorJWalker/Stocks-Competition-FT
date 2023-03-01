using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StocksCompetition.Server.Entities;
using StocksCompetition.Server.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

string dbConnectionString = builder.Configuration.GetConnectionString("MariaDbConnectionString") ?? throw new InvalidOperationException();
builder.Services.AddDbContext<ServerDbContext>(options => 
    options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString)));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ServerDbContext>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
