using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int GuestId { get; set; }

        [ForeignKey("GuestId")]
        public Guest Guest { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public double ReviewRating { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public Rating() { }
        public Rating(int guestId, int hotelId, double reviewRating, string comments)
        {
            GuestId = guestId;
            HotelId = hotelId;
            ReviewRating = reviewRating;
            Comments = comments;
        }
    }
}
