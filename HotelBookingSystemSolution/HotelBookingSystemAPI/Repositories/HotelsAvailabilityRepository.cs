
using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class HotelsAvailabilityRepository : IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate>
    {
        protected readonly HotelBookingContext _context;
        public HotelsAvailabilityRepository(HotelBookingContext context) 
        {
            _context = context;
        }

        public async Task<HotelAvailabilityByDate> Add(HotelAvailabilityByDate item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<HotelAvailabilityByDate> Delete(int key1, DateTime key2)
        {
            try
            {
                var hotelAvailability = await Get(key1,key2);
                _context.Entry<HotelAvailabilityByDate>(hotelAvailability).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return hotelAvailability;

            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("HotelAvailability");
            }
        }

        public async Task<HotelAvailabilityByDate> Get(int key1, DateTime key2)
        {
            var hotelAvailability = await _context.HotelAvailabilityByDates.SingleOrDefaultAsync(h => h.HotelId == key1 && h.Date == key2);
            if (hotelAvailability == null)
                throw new ObjectNotAvailableException("HotelAvailability");
            return hotelAvailability;
        }

        public async Task<IEnumerable<HotelAvailabilityByDate>> Get()
        {
            var hotelAvailability = await _context.HotelAvailabilityByDates.ToListAsync();
            return hotelAvailability;
        }

        public async Task<HotelAvailabilityByDate> Update(HotelAvailabilityByDate item)
        {
            try
            {
                if (await Get(item.HotelId,item.Date) != null)
                {
                    _context.Entry<HotelAvailabilityByDate>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("HotelAvailability");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("HotelAvailability"); 
            }
        }
    }
}
