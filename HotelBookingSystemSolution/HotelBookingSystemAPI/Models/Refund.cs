using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Refund
    {
        [Key]
        public int RefundId { get; set; }
        public int GuestId { get; set; }
        [ForeignKey("GuestId")]
        public Guest Guest { get; set; }

        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Booking Booking { get; set; }

        public double RefundAmount { get; set; }

        public Refund(int guestId, int bookId, double refundAmount)
        {
            GuestId = guestId;
            BookId = bookId;
            RefundAmount = refundAmount;
        }
    }
}
