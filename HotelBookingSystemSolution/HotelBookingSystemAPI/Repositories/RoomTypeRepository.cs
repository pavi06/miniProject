using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class RoomTypeRepository : AbstractRepository<int, RoomType>
    {
        public RoomTypeRepository(HotelBookingContext context) : base(context)
        {
        }

        public override Task<RoomType> Add(RoomType item)
        {
            if (_context.RoomTypes.Any(r => r.Type == item.Type && r.Occupancy == item.Occupancy
            && r.HotelId == item.HotelId))
            {
                throw new ObjectAlreadyExistsException("RoomType");
            }
            return base.Add(item);
        }

        public override async Task<RoomType> Delete(int key)
        {
            try
            {
                var roomType = await Get(key);
                _context.Remove(roomType);
                await _context.SaveChangesAsync();
                return roomType;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("RoomType");
            }
        }

        public override async Task<RoomType> Get(int key)
        {
            var roomType = await _context.RoomTypes.Include(h=>h.Hotel).SingleOrDefaultAsync(r => r.RoomTypeId == key);
            if (roomType != null)
            {
                return roomType;
            }
            throw new ObjectNotAvailableException("RoomType");
        }

        public override async Task<IEnumerable<RoomType>> Get()
        {
            var roomTypes = await _context.RoomTypes.Include(h => h.Hotel).ToListAsync();
            return roomTypes;

        }

        public override async Task<RoomType> Update(RoomType item)
        {
            try
            {
                if (await Get(item.RoomTypeId) != null)
                {
                    _context.Entry<RoomType>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("RoomType");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("RoomType");
            }

        }

    }
}
