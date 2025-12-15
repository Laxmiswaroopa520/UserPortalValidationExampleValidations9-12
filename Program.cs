using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Filters;
// 🔹 Other Services
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models.Config;
using UserPortalValdiationsDBContext.Repository.Implementations;
// 🔹 Repositories & UoW
using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Services;
using UserPortalValdiationsDBContext.Services.Implementations;
// 🔹 Services
using UserPortalValdiationsDBContext.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Controllers + Global Filters
// ---------------------------
=======
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Repository.Implementations;
using UserPortalValdiationsDBContext.Services.Interfaces;
using UserPortalValdiationsDBContext.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Global Filters
>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ErrorHandlingFilter>();
    options.Filters.Add<LoggingActionFilter>();
    options.Filters.Add<ResponseResultFilter>();
    options.Filters.Add<ActionValidationFilter>();
});
<<<<<<< HEAD
builder.Services.Configure<TwilioSettings>(
    builder.Configuration.GetSection("Twilio"));            //Twilio Settings for role based access
// This binds appsettings.json → TwilioSettings class
// ---------------------------
// Authentication & Authorization
// ---------------------------
=======

// Authentication
>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

<<<<<<< HEAD
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
});

// ---------------------------
// Database Context
// ---------------------------
=======
// DbContext
>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

<<<<<<< HEAD
// ---------------------------
// Repositories
// ---------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IUserActivityRepository, UserActivityRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
// Audit
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<ISmsService, TwilioSmsService>();            //Sms service (Twilio Service)


// ---------------------------
// Unit Of Work
// ---------------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ---------------------------
// Application Services
// ---------------------------
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAddressService, AddressService>();
=======
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
>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IUserActivityService, UserActivityService>();
<<<<<<< HEAD
builder.Services.AddScoped<IAuthService, AuthService>();



// ---------------------------
// Filters (DI)
// ---------------------------
=======
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IAuditService, AuditService>();


// Filters
>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc
builder.Services.AddScoped<LoggingActionFilter>();
builder.Services.AddScoped<AuditingFilter>();
builder.Services.AddScoped<ErrorHandlingFilter>();
builder.Services.AddScoped<ActionValidationFilter>();
builder.Services.AddScoped<ResponseResultFilter>();
<<<<<<< HEAD

// ---------------------------
// Caching & Supporting Services
// ---------------------------
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ResultCacheFilter>();
builder.Services.AddMemoryCache(); // required
builder.Services.AddSingleton<ICacheService, CacheService>();

// ---------------------------
// Build App
// ---------------------------
=======
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ResultCacheFilter>();

// Build & pipeline
>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc
var app = builder.Build();

// ---------------------------
// Middleware Pipeline
// ---------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
<<<<<<< HEAD
//app.UseAuthorization();
builder.Services.AddAuthorization(options =>
=======
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
>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc
{
    // Role-based
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));

    // Policy-based: Admin must be 2FA verified
    options.AddPolicy("Admin2FA", policy =>
        policy.RequireAssertion(ctx =>
            ctx.User.IsInRole("Admin") &&
            ctx.User.HasClaim("Is2FAVerified", "true")
        ));

    // Manager policy: OTP + IP check
    options.AddPolicy("ManagerExtraCheck", policy =>
        policy.RequireAssertion(ctx =>
            ctx.User.IsInRole("Manager") &&
            ctx.User.HasClaim("Is2FAVerified", "true") &&
            ctx.User.HasClaim("LoginIP", ctx.User.FindFirst("LoginIP")?.Value ?? "")
        ));
});


// ---------------------------
// Routes
// ---------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");

app.Run();
































