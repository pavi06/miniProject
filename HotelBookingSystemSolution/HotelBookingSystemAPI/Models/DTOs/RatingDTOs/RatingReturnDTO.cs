namespace HotelBookingSystemAPI.Models.DTOs.RatingDTOs
{
    public class RatingReturnDTO
    {
        public int RatingId { get; set; }
        public string GuestName { get; set; }
        public double RatingPoints { get; set; }
        public string Feedback { get; set; }
        public DateTime RatingProvidedDate { get; set; }

        public RatingReturnDTO(int ratingId, double ratingPoints, string feedback, string guestName, DateTime ratingProvidedDate)
        {
            RatingId = ratingId;
            RatingPoints = ratingPoints;
            Feedback = feedback;
            GuestName = guestName;
            RatingProvidedDate = ratingProvidedDate;
        }
    }
}
