using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class BookingRepository : AbstractRepository<int, Booking>
    {
        public BookingRepository(HotelBookingContext context) : base(context)
        {
        }

        public override async Task<Booking> Delete(int key)
        {
            try
            {
                var booking = await Get(key);
                _context.Entry<Booking>(booking).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return booking;

            }
            catch (ObjectNotAvailableException)
            {
                throw ;
            }
        }

        public override async Task<Booking> Get(int key)
        {
            var booking = await _context.Bookings.Include(br => br.RoomsBooked).SingleOrDefaultAsync(b => b.BookId == key);
            if (booking == null)
                throw new ObjectNotAvailableException("Booking");
            return booking;
        }

        public override async Task<IEnumerable<Booking>> Get()
        {
            var bookings = await _context.Bookings.Include(br => br.RoomsBooked).ToListAsync();
            return bookings;

        }

        public override async Task<Booking> Update(Booking item)
        {
            try
            {
                if (await Get(item.BookId) != null)
                {
                    _context.Entry<Booking>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Booking");
            }
            catch (ObjectNotAvailableException)
            {
                throw ;
            }

        }

    }
}
