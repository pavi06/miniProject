using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class PersonRepository : IRepository<int, Person>
    {
        protected readonly HotelBookingContext _context;
        public PersonRepository(HotelBookingContext context)
        {
            _context = context;
        }
        public async Task<Person> Add(Person item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Person> Delete(int key)
        {
            var person = await Get(key);
            if (person != null)
            {
                _context.Remove(person);
                await _context.SaveChangesAsync(true);
                return person;
            }
            throw new ObjectNotAvailableException("Person not available!");
        }

        public virtual Task<Person> Get(int key)
        {
            var person = _context.Person.FirstOrDefaultAsync(e => e.PersonId == key);
            return person;
        }

        public virtual async Task<IEnumerable<Person>> Get()
        {
            var people = await _context.Person.ToListAsync();
            return people;

        }

        public async Task<Person> Update(Person item)
        {
            var people = await Get(item.PersonId);
            if (people != null)
            {
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                return people;
            }
            throw new ObjectNotAvailableException("Person not yet Registered");
        }
    }
}
