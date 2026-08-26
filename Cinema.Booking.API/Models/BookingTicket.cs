namespace Cinema.Booking.API.Models
{
    public class BookingTicket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerEmail { get; set; } = string.Empty;
        public string MovieName { get; set; } = string.Empty;
        public string SeatType { get; set; } = string.Empty;
        public double FinalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
