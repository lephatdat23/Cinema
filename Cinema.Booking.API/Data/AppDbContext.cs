using Cinema.Booking.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Booking.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<BookingTicket> BookingTickets => Set<BookingTicket>();
    }
}
