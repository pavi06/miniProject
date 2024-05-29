namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class RoomTypeDescriptionDTO
    {
        public string Type { get; set; }
        public string Images { get; set; }
        public int Occupancy { get; set; } 
        public int CotsAvailable { get; set; }
        public string Amenities { get; set; }

        public RoomTypeDescriptionDTO(string type, string images, int occupancy, int cotsAvailable, string amenities)
        {
            Type = type;
            Images = images;
            Occupancy = occupancy;
            CotsAvailable = cotsAvailable;
            Amenities = amenities;
        }
    }
}
