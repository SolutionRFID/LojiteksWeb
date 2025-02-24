using LojiteksWeb.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC & Session Servisini Ekleyin
builder.Services.AddControllersWithViews();

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
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi (30 dakika)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HttpClient ve AuthService Servisini Ekleyin
builder.Services.AddHttpClient<AuthApiService>();
builder.Services.AddScoped<AuthApiService>();

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

app.UseAuthorization();

// Varsayılan Route Yapısı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login2}/{id?}");

// API Controller'larını Attribute Routing ile çalıştırın
app.MapControllers();

app.Run();
