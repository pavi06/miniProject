namespace HotelBookingSystemAPI.Models.DTOs
{
    public class HotelReturnDTO
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int TotalNoOfRooms { get; set; }
        public bool IsAvailable { get; set; }
        public double? Rating { get; set; }
        public string Amenities { get; set; }
        public string Restrictions { get; set; }
    }
}
