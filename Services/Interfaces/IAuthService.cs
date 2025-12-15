using Microsoft.AspNetCore.Http;
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface IAuthService
    {
        Task SignInAsync(User user, HttpContext httpContext);
        Task SignOutAsync(HttpContext httpContext);
    }
}


//no repo needed for this code .Because,ere:
/*NO repository is needed for authentication cookies & claims.

Why?

Authentication = infrastructure concern

No DB access here

No EF / DbContext

Just HTTP + Claims + Cookies

👉 So AuthService stays ONLY in Service layer
👉 No Repository for Auth

This is the correct professional decision */