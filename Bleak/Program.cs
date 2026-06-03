using Bleak;
using Bleak.Database;
using Bleak.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{    
    options.UseSqlite("Data Source=database.sqlite3");
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/account/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddIdentity<AppUser, AppRole>(options =>{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit           = false;  
    options.Password.RequireLowercase       = false;
    options.Password.RequireUppercase       = false;
    options.User.RequireUniqueEmail = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddActionService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();


app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
