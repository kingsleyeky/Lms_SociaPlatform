using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.Models.Security;
using LMS_Project.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Build.Exceptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISettings, SettingsRepo>();
builder.Services.AddScoped<IHome, HomeRepo>(); 
builder.Services.AddScoped<IAccount,AccountRepo>();
builder.Services.AddScoped<IMedia,MediaRepo>();
builder.Services.AddScoped<IDocUpload,DocUpload>(); 
builder.Services.AddScoped<ISchedule,ScheduleRepo>(); 
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmailAddress", policy => policy.RequireClaim("EmailAddress"));
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.LoginPath = new PathString("/Home/Login");
        options.ExpireTimeSpan = TimeSpan.FromSeconds(15);
        options.EventsType = typeof(CustomCookieAuthenticationEvents);
    });

builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(10);
});
builder.Services.AddTransient(typeof(CustomCookieAuthenticationEvents));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

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
app.UseAuthentication();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
