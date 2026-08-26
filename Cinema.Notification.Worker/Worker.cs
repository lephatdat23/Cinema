using StackExchange.Redis;

namespace Cinema.Notification.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConnectionMultiplexer _redis;

        public Worker(ILogger<Worker> logger, IConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Processing Service đang lắng nghe Event từ Redis...");

            var sub = _redis.GetSubscriber();

            // Lắng nghe channel "TicketBookedChannel"
            await sub.SubscribeAsync("TicketBookedChannel", (channel, message) =>
            {
                _logger.LogInformation($"[RECEIVED MESSAGE] Đã nhận dữ liệu đặt vé: {message}");
                _logger.LogInformation("--> Tiến hành gửi Email xác nhận vé cho khách hàng thành công!");
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
