using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class BookedRoomsRepository : IRepositoryForCompositeKey<int,int,BookedRooms>
    {
        protected readonly HotelBookingContext _context;
        public BookedRoomsRepository(HotelBookingContext context) 
        {
            _context = context;
        }

        public async Task<BookedRooms> Add(BookedRooms item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<BookedRooms> Delete(int key1, int key2)
        {
            try
            {
                var hotelAvailability = await Get(key1, key2);
                _context.Entry<BookedRooms>(hotelAvailability).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return hotelAvailability;

            }
            catch (ObjectNotAvailableException)
            {
                throw ;
            }
        }

        public async Task<BookedRooms> Get(int key1, int key2)
        {
            var bookedRoom = await _context.BookedRooms.Include(br=>br.Room).SingleOrDefaultAsync(h => h.BookingId == key1 && h.RoomId == key2);
            return bookedRoom;
        }

        public async Task<IEnumerable<BookedRooms>> Get()
        {
            var bookedRooms = await _context.BookedRooms.Include(br => br.Room).ToListAsync();
            return bookedRooms;
        }

        public async Task<BookedRooms> Update(BookedRooms item)
        {
            try
            {
                if (await Get(item.BookingId, item.RoomId) != null)
                {
                    _context.Entry<BookedRooms>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("BookedRoom");
            }
            catch (ObjectNotAvailableException )
            {
                throw ;
            }
        }
    }
}
