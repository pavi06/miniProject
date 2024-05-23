using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Booking
    {
        [Key]
        public int BookId { get; set; }
        public int GuestId { get; set; }
        [ForeignKey("GuestId")]
        public Person Guest { get; set; }
        public int NoOfRooms { get; set; }
        public double TotalAmount { get; set; }
        public double AdvancePayment { get; set; }
        public double? Discount { get; set; }
        public string BookingStatus { get; set; }

        public List<BookedRooms> RoomsBooked { get; set; }

        //public int? PaymentId { get; set; }
        //[ForeignKey("PaymentId")]
        //public Payment Payment { get; set; }

    }
}
