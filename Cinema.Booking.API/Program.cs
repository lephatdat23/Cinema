using Cinema.Booking.API.Data;
using Cinema.Pricing.gRPC.Protos;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Đăng ký SQL Server AppDbContext (BỔ SUNG)
var sqlConn = builder.Configuration.GetConnectionString("DefaultConnection")
              ?? builder.Configuration["ConnectionStrings__DefaultConnection"]
              ?? "Server=cinema-sqlserver,1433;Database=CinemaDb;User Id=sa;Password=Password123!;TrustServerCertificate=True;Encrypt=False;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(sqlConn));

// 2. Đăng ký Redis Connection
var redisConn = builder.Configuration.GetConnectionString("Redis")
                ?? builder.Configuration["ConnectionStrings:Redis"]
                ?? builder.Configuration["ConnectionStrings__Redis"]
                ?? "localhost:6379";

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));

// 3. Đăng ký gRPC Client
var pricingUrl = builder.Configuration["GrpcSettings:PricingUrl"] 
                 ?? builder.Configuration["GrpcSettings__PricingUrl"] 
                 ?? "http://localhost:5001";

builder.Services.AddGrpcClient<PricingProtoService.PricingProtoServiceClient>(o =>
{
    o.Address = new Uri(pricingUrl);
});

var app = builder.Build();

// 4. TỰ ĐỘNG KHỞI TẠO DATABASE TRONG CONTAINER (BỔ SUNG)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated(); // Tự động tạo bảng BookingTickets trong SQL Server
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cinema Booking API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();
app.MapControllers();

app.Run();