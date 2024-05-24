using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class UserRepository : AbstractRepository<int, User>
    {
        public UserRepository(HotelBookingContext context) : base(context)
        {
        }

        public override async Task<User> Delete(int key)
        {
                try
                {
                    var user = await Get(key);
                    _context.Remove(user);
                    await _context.SaveChangesAsync();
                    return user;
                }
                catch (ObjectNotAvailableException e)
                {
                    throw e;
                }
            
        }

        public override async Task<User> Get(int key)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.GuestId == key);
            if(user != null)
            {
                return user;
            }
            throw new ObjectNotAvailableException("User");
        }

        public override async Task<IEnumerable<User>> Get()
        {
            var user = await _context.Users.ToListAsync();
            return user;
        }

        public override async Task<User> Update(User item)
        {
            try
            {
                if (await Get(item.GuestId) != null)
                {
                    _context.Entry<User>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("User");
            }
            catch(ObjectNotAvailableException e)
            {
                throw e;
            }
            
        }
    }
}
