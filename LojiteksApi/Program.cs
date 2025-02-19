using LojiteksDataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 📌 MSSQL Veritabanı Bağlantısını Tanımla
string connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging(true); // Hata ayıklamak için
});

// 🔹 MVC Controller Servisini Ekle
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

// 🔹 Yetkilendirme Servisini Ekle
builder.Services.AddAuthorization();

// 🔹 Swagger ve API Belgeleri
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Hataları ayrıntılı görmek için hata middleware ekleyin
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var error = new { message = "Internal Server Error", details = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error.Message };
        await context.Response.WriteAsJsonAsync(error);
    });
});

// 🔹 Middleware Katmanlarını Doğru Sırayla Kullan
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lojiteks API v1");
    c.RoutePrefix = "swagger"; // Swagger UI'yi /swagger/ adresine yönlendir
});

// 🔹 Controller Haritalaması
app.MapControllers();

app.Run();
