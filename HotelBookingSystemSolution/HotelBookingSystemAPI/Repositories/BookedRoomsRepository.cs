using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class BookedRoomsRepository : AbstractRepository<(int, int), BookedRooms>
    {
        public BookedRoomsRepository(HotelBookingContext context) : base(context)
        {
        }

        public override async Task<BookedRooms> Delete((int, int) key)
        {
            try
            {
                var hotelAvailability = await Get(key);
                _context.Entry<BookedRooms>(hotelAvailability).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return hotelAvailability;

            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }
        }

        public override async Task<BookedRooms> Get((int, int) key)
        {
            var bookedRoom = await _context.BookedRooms.SingleOrDefaultAsync(h => h.BookingId == key.Item1 && h.RoomId == key.Item2);
            return bookedRoom;
        }

        public override async Task<IEnumerable<BookedRooms>> Get()
        {
            var bookedRooms = await _context.BookedRooms.ToListAsync();
            return bookedRooms;
        }

        public override async Task<BookedRooms> Update(BookedRooms item)
        {
            try
            {
                if (await Get((item.BookingId, item.RoomId)) != null)
                {
                    _context.Entry<BookedRooms>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("BookedRoom");
            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }
        }
    }
}
