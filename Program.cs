using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetMVCLearning.Data;
using NetMVCLearning.Helpers;
using NetMVCLearning.Models;
using NetMVCLearning.Repository;
using NetMVCLearning.Repository.Interfaces;
using NetMVCLearning.Services;
using NetMVCLearning.Services.Implementation;

namespace NetMVCLearning;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
        });
        builder.Services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddMemoryCache();
        builder.Services.AddSession();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
        
        builder.Services.AddScoped<IClubRepository, ClubRepository>();
        builder.Services.AddScoped<IRaceRepository, RaceRepository>();
        builder.Services.Configure<CloudanarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
        builder.Services.AddScoped<IPhotoService, PhotoService>();

        var app = builder.Build();

        if (args.Length == 1 && args[0].ToLower() == "seeddata")
        {
            // Seed.SeedData(app);
            Seed.SeedUsersAndRolesAsync(app).GetAwaiter().GetResult();
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}