
using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class HotelsAvailabilityRepository : AbstractRepository<(int, DateTime), HotelAvailabilityByDate>
    {
        public HotelsAvailabilityRepository(HotelBookingContext context) : base(context)
        {
        }

        public override async Task<HotelAvailabilityByDate> Delete((int, DateTime) key)
        {
            try
            {
                var hotelAvailability = await Get(key);
                _context.Entry<HotelAvailabilityByDate>(hotelAvailability).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return hotelAvailability;

            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }
        }

        public override async Task<HotelAvailabilityByDate> Get((int, DateTime) key)
        {
            var hotelAvailability = await _context.HotelAvailabilityByDates.SingleOrDefaultAsync(h => h.HotelId == key.Item1 && h.Date == key.Item2);
            return hotelAvailability;
        }

        public override async Task<IEnumerable<HotelAvailabilityByDate>> Get()
        {
            var hotelAvailability = await _context.HotelAvailabilityByDates.ToListAsync();
            return hotelAvailability;
        }

        public override async Task<HotelAvailabilityByDate> Update(HotelAvailabilityByDate item)
        {
            try
            {
                if (await Get((item.HotelId,item.Date)) != null)
                {
                    _context.Entry<HotelAvailabilityByDate>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Hotel");
            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }
        }
    }
}
