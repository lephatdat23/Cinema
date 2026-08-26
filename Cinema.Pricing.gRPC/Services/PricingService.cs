using Cinema.Pricing.gRPC.Protos;
using Grpc.Core;

namespace Cinema.Pricing.gRPC.Services
{
    public class PricingService : PricingProtoService.PricingProtoServiceBase
    {
        public override Task<CalculatePriceResponse> CalculateTicketPrice(
        CalculatePriceRequest request, ServerCallContext context)
        {
            double price = request.BasePrice;

            if (request.SeatType.Equals("VIP", StringComparison.OrdinalIgnoreCase))
                price += 20000;
            else if (request.SeatType.Equals("Couple", StringComparison.OrdinalIgnoreCase))
                price += 40000;

            if (request.IsWeekend)
                price *= 1.2;

            return Task.FromResult(new CalculatePriceResponse { FinalPrice = price });
        }
    }
}
