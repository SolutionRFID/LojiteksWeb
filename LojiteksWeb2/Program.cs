using LojiteksWeb2.Services;
using Skote.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 MVC & Session Servisini Ekleyin
builder.Services.AddControllersWithViews();

// 🔹 Session Servisini Ekle
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi (30 dakika)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 🔹 HttpClient ve AuthService Servisini Ekleyin
builder.Services.AddHttpClient<AuthApiService>();
builder.Services.AddScoped<AuthApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 🔹 Middleware Katmanları (Sıra Önemlidir)
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 🔹 Session Kullanımı (Doğru Sırada Olsun)
app.UseSession();

app.UseAuthorization();

// 🔹 Varsayılan Route Yapısı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login2}/{id?}");

app.Run();
