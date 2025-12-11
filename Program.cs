using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Repository.Implementations;
using UserPortalValdiationsDBContext.Services.Interfaces;
using UserPortalValdiationsDBContext.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Global Filters
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ErrorHandlingFilter>();
    options.Filters.Add<LoggingActionFilter>();
    options.Filters.Add<ResponseResultFilter>();
    options.Filters.Add<ActionValidationFilter>();
});

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository & Unit Of Work Registrations

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IHobbyRepository, HobbyRepository>();


// Services (Business Layer)

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IUserActivityService, UserActivityService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IAuditService, AuditService>();


// Filters
builder.Services.AddScoped<LoggingActionFilter>();
builder.Services.AddScoped<AuditingFilter>();
builder.Services.AddScoped<ErrorHandlingFilter>();
builder.Services.AddScoped<ActionValidationFilter>();
builder.Services.AddScoped<ResponseResultFilter>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ResultCacheFilter>();

// Build & pipeline
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

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