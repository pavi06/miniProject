using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class GuestRepository : AbstractRepository<int,Guest>
    {
        public GuestRepository(HotelBookingContext context) : base(context)
        {
        }

        public override Task<Guest> Add(Guest item)
        {
            if (_context.Guests.Any(p =>p.Name==item.Name || p.Email == item.Email))
            {
                throw new ObjectAlreadyExistsException("User");
            }                
            return base.Add(item);
        }

        public override async Task<Guest> Delete(int key)
        {
            try
            {
                var person = await Get(key);
                _context.Remove(person);
                await _context.SaveChangesAsync();
                return person;
            }
            catch(ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("User");
            }
        }

        public override async Task<Guest> Get(int key)
        {
            var person = await _context.Guests.SingleOrDefaultAsync(p => p.GuestId == key);
            if (person != null)
            {
                return person;
            }
            throw new ObjectNotAvailableException("User");
        }

        public override async Task<IEnumerable<Guest>> Get()
        {
            var people = await _context.Guests.ToListAsync();
            return people;

        }

        public override async Task<Guest> Update(Guest item)
        {
            try
            {
                if (await Get(item.GuestId) != null)
                {
                    _context.Entry<Guest>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("User");
            }
            catch(ObjectNotAvailableException)
            {
                throw ;
            }
            
        }
    }
}
