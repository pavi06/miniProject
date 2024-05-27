using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public abstract class AbstractRepository<K, T> : IRepository<K, T>
    {
        protected readonly HotelBookingContext _context;
        public AbstractRepository(HotelBookingContext context)
        {
            _context = context;
        }

        public virtual async Task<T> Add(T item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync(true);
            return item;
        }

        public abstract Task<T> Delete(K key);

        public abstract Task<T> Get(K key);

        public abstract Task<IEnumerable<T>> Get();

        public abstract Task<T> Update(T item);
    }
}
