namespace HotelBookingSystemAPI.Models.DTOs.InsertDTOs
{
    public class HotelReturnDTO
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public bool IsAvailable { get; set; }
        public double Rating { get; set; }
        public string Amenities { get; set; }
        public string Restrictions { get; set; }

        public HotelReturnDTO(int hotelId, string name, string address, string city, double rating, string amenities, string restrictions, bool isAvailable, List<Rating> ratings, List<RoomType> roomTypes)
        {
            HotelId = hotelId;
            Name = name;
            Address = address;
            City = city;
            Rating = rating;
            Amenities = amenities;
            Restrictions = restrictions;
            IsAvailable = isAvailable;

        }
    }
}
