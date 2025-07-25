using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Persistance.Context;
using ShoppingCart.Policies.Role;
using ShoppingCart.ProgramConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
    // For MVC
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Error/Unauthorized";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Role",
        policyBuilder =>
        {
            policyBuilder.RequireAuthenticatedUser();
            policyBuilder.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
            policyBuilder.AddRequirements(
                new RoleRequirement()
            );
        });
});

builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddScoped<IAuthorizationHandler, RoleHandler>();
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Populate;
    options.UseMemberCasing();
});

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();
    db.Database.Migrate();
}

app.Run();
