using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class RoomRepository : AbstractRepository<int,Room>
    {
        public RoomRepository(HotelBookingContext context) : base(context)
        {
        }

        public override async Task<Room> Delete(int key)
        {
            try
            {
                var room = await Get(key);
                _context.Entry<Room>(room).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return room;

            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }
        }

        public override async Task<Room> Get(int key)
        {
            var room = await _context.Rooms.SingleOrDefaultAsync(r => r.RoomId == key);
            if (room == null)
                throw new ObjectNotAvailableException("Room");
            return room;
        }

        public override async Task<IEnumerable<Room>> Get()
        {
            var room = await _context.Rooms.ToListAsync();
            return room;

        }

        public override async Task<Room> Update(Room item)
        {
            try
            {
                if (await Get(item.HotelId) != null)
                {
                    _context.Entry<Room>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Room");
            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }

        }
    }
}
