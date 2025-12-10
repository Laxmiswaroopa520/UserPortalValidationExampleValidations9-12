using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Services;
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();                    //enables razor view files(.cshtml) :to be recompiled .so that, it will show the changes without restarting the app.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ErrorHandlingFilter>();
    options.Filters.Add<LoggingActionFilter>();
    options.Filters.Add<AuditingFilter>();
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
builder.Services.AddScoped<ResultCacheFilter>();
builder.Services.AddScoped<ActionValidationFilter>();
builder.Services.AddScoped<ResponseResultFilter>();

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
