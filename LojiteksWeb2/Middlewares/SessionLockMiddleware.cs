using LojiteksWeb.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LojiteksWeb.Middlewares
{
    public class SessionLockMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionLockMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var requestPath = context.Request.Path;

                // Eğer istek login sayfasına aitse, middleware'i atla.
                if (context.Request.Path.StartsWithSegments("/Auth"))
                {
                    await _next(context);
                    return;
                }

                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    await _next(context);
                    return;
                }

                // Kullanıcının giriş yapıp yapmadığını kontrol edin
                var userJson = context.Session.GetString("Sessions");
                var Sessions = JsonSerializer.Deserialize<Sessions>(userJson);

                if (Sessions == null)
                {
                    context.Response.Redirect("/Auth/Login2");
                    return;
                }

                if (Sessions.IsLocked == 1)
                {
                    if (!context.Request.Path.StartsWithSegments("/Auth/LockScreen2"))
                    {
                        context.Response.Redirect("/Auth/LockScreen2");
                        return;
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.Redirect("/Auth/Login2");
                return;
            }

        }
    }
}
