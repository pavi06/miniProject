using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class RatingRepository : AbstractRepository<int, Rating>
    {
        public RatingRepository(HotelBookingContext context) : base(context)
        {
        }

        public override async Task<Rating> Delete(int key)
        {
            try
            {
                var rating = await Get(key);
                _context.Entry<Rating>(rating).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return rating;

            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Rating");
            }
        }

        public override async Task<Rating> Get(int key)
        {
            var rating = await _context.Ratings.Include(r=>r.Guest).SingleOrDefaultAsync(h => h.RatingId == key);
            if (rating == null)
                throw new ObjectNotAvailableException("Rating");
            return rating;
        }

        public override async Task<IEnumerable<Rating>> Get()
        {
            var rating = await _context.Ratings.Include(r => r.Guest).ToListAsync();
            return rating;

        }

        public override async Task<Rating> Update(Rating item)
        {
            try
            {
                if (await Get(item.RatingId) != null)
                {
                    _context.Entry<Rating>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Rating");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Rating");
            }

        }
    }
}
