using Microsoft.Extensions.Options;
using URLShortener.Data;
using StackExchange.Redis;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5043");


// Cấu hình MongoDB
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<MongoDbContext>();

var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString");
Console.WriteLine($"🔗 Connecting to Redis at: {redisConnectionString}");

ConnectionMultiplexer redis = null;
int maxRetry = 5; // Thử kết nối Redis tối đa 5 lần
for (int i = 0; i < maxRetry; i++)
{
    try
    {
        redis = ConnectionMultiplexer.Connect(redisConnectionString);
        Console.WriteLine("✅ Redis connected successfully!");
        break;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Redis connection failed (Attempt {i + 1}/{maxRetry}): {ex.Message}");
        Thread.Sleep(2000); // Chờ 2 giây trước khi thử lại
    }
}

if (redis == null)
{
    Console.WriteLine("🚨 Failed to connect to Redis after multiple attempts. Exiting...");
    Environment.Exit(1); // Dừng ứng dụng nếu không kết nối được Redis
}

builder.Services.AddSingleton(redis.GetDatabase());

// Đăng ký dịch vụ URL Shortener
builder.Services.AddScoped<URLShortener.Services.UrlService>();

// Thêm Controller & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
