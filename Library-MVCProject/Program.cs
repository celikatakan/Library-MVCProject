using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);       // Create a new builder for the web application

builder.Services.AddControllersWithViews();             // Add services to the container, including support for controllers and views

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/");

});




var app = builder.Build();                  // Build the web application based on the configuration

app.UseAuthentication();

app.UseStaticFiles();    // used for wwwroot.

app.MapControllerRoute(
    name: "default",                                 // This route is named "default".
    pattern: "{controller=Home}/{action=Index}/{id?}"      // URL pattern: by default, the "Home" controller and "Index" action are called.  
    );

app.Run();
