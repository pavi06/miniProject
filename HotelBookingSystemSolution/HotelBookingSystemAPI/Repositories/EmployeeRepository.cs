using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class EmployeeRepository : AbstractRepository<int,HotelEmployee>
    {
        public EmployeeRepository(HotelBookingContext context) : base(context)
        {
        }

        public override Task<HotelEmployee> Add(HotelEmployee item)
        {
            if (_context.Employees.Any(p => p.Name == item.Name && p.Email == item.Email
            && p.PhoneNumber == item.PhoneNumber && p.Address == item.Address))
            {
                throw new ObjectAlreadyExistsException("User");
            }
            return base.Add(item);
        }

        public override async Task<HotelEmployee> Delete(int key)
        {
            try
            {
                var person = await Get(key);
                _context.Remove(person);
                await _context.SaveChangesAsync();
                return person;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("User");
            }
        }

        public override async Task<HotelEmployee> Get(int key)
        {
            var person = await _context.Employees.SingleOrDefaultAsync(p => p.EmpId == key);
            if (person != null)
            {
                return person;
            }
            throw new ObjectNotAvailableException("User");
        }

        public override async Task<IEnumerable<HotelEmployee>> Get()
        {
            var people = await _context.Employees.ToListAsync();
            return people;

        }

        public override async Task<HotelEmployee> Update(HotelEmployee item)
        {
            try
            {
                if (await Get(item.EmpId) != null)
                {
                    _context.Entry<HotelEmployee>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("User");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }

    }
}
