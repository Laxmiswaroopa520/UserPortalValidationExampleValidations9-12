using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Add Controllers with Global Filters
// ---------------------------
builder.Services.AddControllersWithViews(options =>
{
    // Global filters
    options.Filters.Add<ErrorHandlingFilter>();      // Global exception handling
    options.Filters.Add<LoggingActionFilter>();      // Logs all actions
    options.Filters.Add<ResponseResultFilter>();     // Wrap API responses in standard format
    options.Filters.Add<ActionValidationFilter>();   // Validate ModelState globally
});

// ---------------------------
// Authentication
// ---------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

// ---------------------------
// Database Context
// ---------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------
// App Services
// ---------------------------
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserActivityService, UserActivityService>();

// ---------------------------
// Filters with Dependency Injection
// ---------------------------

// LoggingActionFilter depends on ILogger<LoggingActionFilter> (automatically provided by DI)
builder.Services.AddScoped<LoggingActionFilter>();

// AuditingFilter depends on IAuditService and ILogger<AuditingFilter> (ILogger automatically injected)
builder.Services.AddScoped<AuditingFilter>();

// ErrorHandlingFilter depends on ILogger<ErrorHandlingFilter>
builder.Services.AddScoped<ErrorHandlingFilter>();

// ActionValidationFilter has no dependencies
builder.Services.AddScoped<ActionValidationFilter>();

// ResultCacheFilter depends on IMemoryCache; default duration = 10 sec
builder.Services.AddMemoryCache(); // required for ResultCacheFilter
builder.Services.AddScoped<ResultCacheFilter>();

// ResponseResultFilter has no dependencies (or inject what it needs)
builder.Services.AddScoped<ResponseResultFilter>();

// ---------------------------
// Supporting Services
// ---------------------------
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<IAuditService, AuditService>();

// ---------------------------
// Build and Configure App
// ---------------------------
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");

app.Run();




/*using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Services;
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();                    //enables razor view files(.cshtml) :to be recompiled .so that, it will show the changes without restarting the app.
builder.Services.AddControllersWithViews(options =>
{
    // Global Filters
    options.Filters.Add<ErrorHandlingFilter>();     // Global exception handling
    options.Filters.Add<LoggingActionFilter>();      // Logs all actions
    options.Filters.Add<ResponseResultFilter>();     // Wraps API responses in standard format
});

// Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // redirect here if not authenticated
        options.LogoutPath = "/Account/Logout"; // optional
    });
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// register services for filters
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<IAuditService, AuditService>();

builder.Services.AddScoped<ResponseResultFilter>();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<ActionValidationFilter>();
builder.Services.AddScoped<AuditingFilter>();
builder.Services.AddScoped<ResultCacheFilter>();   // already injects IMemoryCache




//register services for normal services.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserActivityService, UserActivityService>();

//used for sending real emails..



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");

app.Run();
*/