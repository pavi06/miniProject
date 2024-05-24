using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class HotelRepository : IRepository<int, Hotel>
    {
        protected readonly HotelBookingContext _context;
        public HotelRepository(HotelBookingContext context)
        {
            _context = context;
        }
        public async Task<Hotel> Add(Hotel item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Hotel> Delete(int key)
        {
            var hotel = await Get(key);
            if(hotel == null)
                throw new ObjectNotAvailableException("Hotel not available");
            _context.Remove(hotel);
            await _context.SaveChangesAsync(); 
            return hotel;
        }

        public virtual Task<Hotel> Get(int key)
        {
            var hotel = _context.Hotels.FirstOrDefaultAsync(e => e.HotelId == key);
            return hotel;
        }

        public virtual async Task<IEnumerable<Hotel>> Get()
        {
            var hotels = await _context.Hotels.ToListAsync();
            return hotels;

        }

        public async Task<Hotel> Update(Hotel item)
        {
            var hotel = await Get(item.HotelId);
            if (hotel != null)
            {
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                return hotel;
            }
            throw new ObjectNotAvailableException("Hotel not available!");
        }
    }
}
