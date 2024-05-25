namespace HotelBookingSystemAPI.Models.DTOs
{
    public class HotelReturnDTO
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int NoOfRoomsAvailable { get; set; }
        public bool IsAvailable { get; set; }
        public double Rating { get; set; }
        public string Amenities { get; set; }
        public string Restrictions { get; set; }

        public HotelReturnDTO(int hotelId, string name, string address, string city, int noOfRoomsAvailable, bool isAvailable, double rating, string amenities, string restrictions)
        {
            HotelId = hotelId;
            Name = name;
            Address = address;
            City = city;
            NoOfRoomsAvailable = noOfRoomsAvailable;
            IsAvailable = isAvailable;
            Rating = rating;
            Amenities = amenities;
            Restrictions = restrictions;
        }
    }
}
