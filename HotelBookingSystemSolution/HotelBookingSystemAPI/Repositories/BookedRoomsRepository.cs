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
                var bookedRoom = await Get(key1, key2);
                _context.BookedRooms.Remove(bookedRoom);
                await _context.SaveChangesAsync(true);
                return bookedRoom;

            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("BookedRoom");
            }
        }

        public async Task<BookedRooms> Get(int key1, int key2)
        {
            var bookedRoom = await _context.BookedRooms.Include(br=>br.Room).Include(br => br.Booking).SingleOrDefaultAsync(h => h.RoomId == key1 && h.BookingId == key2);
            return bookedRoom;
        }

        public async Task<IEnumerable<BookedRooms>> Get()
        {
            var bookedRooms = await _context.BookedRooms.Include(br => br.Room).Include(br => br.Booking).ToListAsync();
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
                throw new ObjectNotAvailableException("BookedRoom");
            }
        }
    }
}
