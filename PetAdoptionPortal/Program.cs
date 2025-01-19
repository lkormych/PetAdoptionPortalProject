using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PAPData.Entities;
using PAPData.Entities.Repositories;
using PAPServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PAPContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PAPConnection"),
        x => x.MigrationsAssembly("PAPData")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PAPContext>();

builder.Services.AddAuthentication()
    .AddCookie(options => {
        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    });

builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<PetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();