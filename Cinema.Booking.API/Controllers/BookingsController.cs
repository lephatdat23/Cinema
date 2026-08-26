using Cinema.Booking.API.Data;
using Cinema.Booking.API.Models;
using Cinema.Pricing.gRPC.Protos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
using PricingClient = Cinema.Pricing.gRPC.Protos.PricingProtoService.PricingProtoServiceClient;

namespace Cinema.Booking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly PricingProtoService.PricingProtoServiceClient _grpcClient;
    private readonly IConnectionMultiplexer _redis;
    private readonly AppDbContext _dbContext; // 1. Bổ sung DbContext

    public BookingsController(
        PricingProtoService.PricingProtoServiceClient grpcClient,
        IConnectionMultiplexer redis,
        AppDbContext dbContext) // Inject DbContext vào Constructor
    {
        _grpcClient = grpcClient;
        _redis = redis;
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingRequest request)
    {
        // 1. Gọi gRPC Service tính giá vé
        var priceResponse = await _grpcClient.CalculateTicketPriceAsync(new CalculatePriceRequest
        {
            BasePrice = 80000,
            SeatType = request.SeatType,
            IsWeekend = request.IsWeekend
        });

        // 2. Tạo Model Entity để lưu DB
        var bookingTicket = new BookingTicket
        {
            Id = Guid.NewGuid(),
            CustomerEmail = request.CustomerEmail,
            MovieName = request.MovieName,
            SeatType = request.SeatType,
            FinalPrice = priceResponse.FinalPrice,
            CreatedAt = DateTime.UtcNow
        };

        // 3. THÊM DÒNG NÀY: Lưu thông tin vé thực sự vào SQL Server Database
        _dbContext.BookingTickets.Add(bookingTicket);
        await _dbContext.SaveChangesAsync();

        // 4. Publish message lên Redis Message Broker (Producer)
        var sub = _redis.GetSubscriber();
        await sub.PublishAsync(RedisChannel.Literal("TicketBookedChannel"), JsonSerializer.Serialize(bookingTicket));

        return Ok(new { Message = "Đặt vé thành công!", Details = bookingTicket });
    }
}

public class BookingRequest
{
    public string CustomerEmail { get; set; } = string.Empty;
    public string MovieName { get; set; } = string.Empty;
    public string SeatType { get; set; } = "Standard";
    public bool IsWeekend { get; set; }
}
