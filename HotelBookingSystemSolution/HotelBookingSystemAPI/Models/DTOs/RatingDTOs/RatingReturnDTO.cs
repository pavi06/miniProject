namespace HotelBookingSystemAPI.Models.DTOs.RatingDTOs
{
    public class RatingReturnDTO
    {
        public int RatingId { get; set; }
        public double RatingPoints { get; set; }
        public string Feedback { get; set; }

        public RatingReturnDTO(int ratingId, double ratingPoints, string feedback)
        {
            RatingId = ratingId;
            RatingPoints = ratingPoints;
            Feedback = feedback;
        }
    }
}
