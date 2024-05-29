namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class RoomTypeReturnDTO
    {
        public int RoomTypeId { get; set; }
        public string Type { get; set; }
        public int Occupancy { get; set; }
        public string Images { get; set; }
        public double Amount { get; set; }
        public int CotsAvailable { get; set; }
        public string Amenities { get; set; }
        public double? Discount { get; set; }
        public int HotelId { get; set; }

        public RoomTypeReturnDTO(int roomTypeId, string type,int occupancy,string images, double amount, int cotsAvailable, string amenities, double? discount, int hotelId)
        {
            RoomTypeId = roomTypeId;
            Type = type;
            Occupancy = occupancy;
            Images = images;
            Amount = amount;
            CotsAvailable = cotsAvailable;
            Amenities = amenities;
            Discount = discount;
            HotelId = hotelId;
        }
    }
}
