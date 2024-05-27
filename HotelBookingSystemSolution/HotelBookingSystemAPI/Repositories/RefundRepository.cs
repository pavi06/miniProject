using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repositories
{
    public class RefundRepository : AbstractRepository<int,Refund>
    {
        public RefundRepository(HotelBookingContext context) : base(context)
        {
        }

        public override async Task<Refund> Delete(int key)
        {
            try
            {
                var refund = await Get(key);
                _context.Entry<Refund>(refund).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return refund;

            }
            catch (ObjectNotAvailableException)
            {
                throw ;
            }
        }

        public override async Task<Refund> Get(int key)
        {
            var refund = await _context.Refunds.SingleOrDefaultAsync(r=>r.RefundId == key);
            if (refund == null)
                throw new ObjectNotAvailableException("Refund");
            return refund;
        }

        public override async Task<IEnumerable<Refund>> Get()
        {
            var refunds = await _context.Refunds.ToListAsync();
            return refunds;

        }

        public override async Task<Refund> Update(Refund item)
        {
            try
            {
                if (await Get(item.RefundId) != null)
                {
                    _context.Entry<Refund>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Refund");
            }
            catch (ObjectNotAvailableException)
            {
                throw ;
            }

        }
    }
}
