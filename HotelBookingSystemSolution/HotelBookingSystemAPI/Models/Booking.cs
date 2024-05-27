using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Booking
    {
        [Key]
        public int BookId { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int NoOfRooms { get; set; }
        public double TotalAmount { get; set; }
        public double AdvancePayment { get; set; }
        public double Discount { get; set; }
        public string BookingStatus { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public List<BookedRooms> RoomsBooked { get; set; }

        public int? PaymentId { get; set; }

        public List<Payment> Payments { get; set; }

        public Booking(int guestId, int noOfRooms, double totalAmount, double advancePayment, double discount, string bookingStatus, int? paymentId, int hotelId)
        {
            GuestId = guestId;
            NoOfRooms = noOfRooms;
            TotalAmount = totalAmount;
            AdvancePayment = advancePayment;
            Discount = discount;
            BookingStatus = bookingStatus;
            PaymentId = paymentId;
            HotelId = hotelId;
        }
    }
}
