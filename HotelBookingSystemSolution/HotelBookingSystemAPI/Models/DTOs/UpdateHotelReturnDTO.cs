using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs
{
    public class UpdateHotelReturnDTO
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public int TotalNoOfRooms { get; set; }
        public int NoOfRoomsAvailable { get; set; }
        public double Ratings { get; set; }
        public string? Amenities { get; set; }
        public string? Restrictions { get; set; }

        public UpdateHotelReturnDTO(int hotelId, string name, int totalNoOfRooms, int noOfRoomsAvailable, double ratings, string? amenities, string? restrictions)
        {
            HotelId = hotelId;
            Name = name;
            TotalNoOfRooms = totalNoOfRooms;
            NoOfRoomsAvailable = noOfRoomsAvailable;
            Ratings = ratings;
            Amenities = amenities;
            Restrictions = restrictions;
        }
    }
}
