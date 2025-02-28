using LojiteksWeb.Middlewares; // Middleware sınıfınızın bulunduğu namespace
using Microsoft.OpenApi.Models;
using LojiteksWeb.Migrations;
using LojiteksWeb.Middlewares;
using Microsoft.EntityFrameworkCore;
using LojiteksWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC & Session Servisini Ekleyin
builder.Services.AddControllersWithViews();

// Bağlantı dizesini alıp DbContext'e ekliyoruz.
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// API Endpoint'lerini Desteklemek için Swagger ekleyin
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LojiteksWeb API",
        Version = "v1",
        Description = "API endpoint'lerini içeren LojiteksWeb API dokümantasyonu."
    });
});

// Session Servisini Ekle
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // 2 saatlik session süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

/* HttpClient ve AuthService Servisini Ekleyin
builder.Services.AddHttpClient<AuthApiService>();
builder.Services.AddScoped<AuthApiService>();*/

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // Geliştirme ortamında Swagger'ı kullanın
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LojiteksWeb API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Session Kullanımı
app.UseSession();

// Session Lock Middleware'ini ekleyin (Session'dan sonra, Authorization'dan önce)
app.UseMiddleware<SessionLockMiddleware>();

app.UseAuthorization();

// Varsayılan Route Yapısı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}"
);

// API Controller'larını Attribute Routing ile çalıştırın
app.MapControllers();

app.Run();
