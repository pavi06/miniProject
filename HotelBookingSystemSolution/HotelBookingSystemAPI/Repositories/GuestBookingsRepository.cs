using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class GuestBookingsRepository : GuestRepository
    {
        public GuestBookingsRepository(HotelBookingContext context) : base(context)
        {
        }

        public override async Task<Guest> Get(int key)
        {
            var guest = await _context.Guests.Include(g=>g.Bookings).SingleOrDefaultAsync(p => p.GuestId == key);
            if (guest != null)
            {
                return guest;
            }
            throw new ObjectNotAvailableException("User");
        }

        public override async Task<IEnumerable<Guest>> Get()
        {
            var guest = await _context.Guests.Include(g=>g.Bookings).ToListAsync();
            return guest;

        }
    }
}
