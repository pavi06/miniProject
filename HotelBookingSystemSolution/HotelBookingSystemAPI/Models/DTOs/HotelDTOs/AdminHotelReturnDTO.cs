namespace HotelBookingSystemAPI.Models.DTOs.HotelDTOs
{
    public class AdminHotelReturnDTO
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public bool IsAvailable { get; set; }
        public double Rating { get; set; }
        public int RatingCount { get; set; }
        public string Amenities { get; set; }
        public string Restrictions { get; set; }
        public int TotalNoOfRooms { get; set; }
        public Dictionary<int,string> RoomTypes { get; set; }

        public AdminHotelReturnDTO(int hotelId, string name, string address, string city, double rating,  int ratingCount, string amenities, string restrictions, bool isAvailable, int totalNoOfRooms, Dictionary<int, string> roomTypes)
        {
            HotelId = hotelId;
            Name = name;
            Address = address;
            City = city;
            Rating = rating;
            RatingCount = ratingCount;
            Amenities = amenities;
            Restrictions = restrictions;
            IsAvailable = isAvailable;
            TotalNoOfRooms = totalNoOfRooms;
            RoomTypes = roomTypes;
        }
    }
}
