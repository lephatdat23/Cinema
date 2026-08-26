using Cinema.Pricing.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Bắt buộc Kestrel lắng nghe HTTP/2 trên Docker (Cổng 80)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<PricingService>(); // Hoặc tên service gRPC của bạn
app.MapGet("/", () => "gRPC service is running");

app.Run();
