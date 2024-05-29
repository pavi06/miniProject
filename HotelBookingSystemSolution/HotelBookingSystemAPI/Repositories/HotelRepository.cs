using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class HotelRepository : AbstractRepository<int, Hotel>
    {
        public HotelRepository(HotelBookingContext context) : base(context)
        {
        }

        public override Task<Hotel> Add(Hotel item)
        {
            if (_context.Hotels.Any(h => h.Name == item.Name && h.Address == item.Address
            && h.City == item.City))
            {
                throw new ObjectAlreadyExistsException("Hotel");
            }
            return base.Add(item);
        }

        public override async Task<Hotel> Delete(int key)
        {
            try
            {
                var hotel = await Get(key);
                _context.Entry<Hotel>(hotel).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return hotel;

            }
            catch (ObjectNotAvailableException)
            {
                throw ;
            }
        }

        public override async Task<Hotel> Get(int key)
        {
            var hotel = await _context.Hotels.Include(h=>h.Rooms).Include(h=>h.RoomTypes).Include(h=>h.Ratings).Include(h => h.bookingsForHotel).SingleOrDefaultAsync(h => h.HotelId == key);
            if (hotel == null)
                throw new ObjectNotAvailableException("Hotel");
            return hotel;
        }

        public override async Task<IEnumerable<Hotel>> Get()
        {
            var hotels = await _context.Hotels.Include(h => h.Rooms).Include(h => h.RoomTypes).Include(h => h.Ratings).Include(h => h.bookingsForHotel).ToListAsync();
            return hotels;

        }

        public override async Task<Hotel> Update(Hotel item)
        {
            try
            {
                if (await Get(item.HotelId) != null)
                {
                    _context.Entry<Hotel>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Hotel");
            }
            catch(ObjectNotAvailableException)
            {
                throw ;
            }
            
        }
    }
}
