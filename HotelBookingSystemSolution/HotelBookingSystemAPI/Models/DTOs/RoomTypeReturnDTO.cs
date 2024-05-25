namespace HotelBookingSystemAPI.Models.DTOs
{
    public class RoomTypeReturnDTO
    {
        public int RoomTypeId { get; set; }
        public RoomTypes Type { get; set; }
        public int Occupancy { get; set; }
        public double Amount { get; set; }
        public int CotsAvailable { get; set; }
        public string Amenities { get; set; }
        public double? Discount { get; set; }
        public int HotelId { get; set; }

        public RoomTypeReturnDTO(int roomTypeId, int occupancy, double amount, int cotsAvailable, string amenities, double? discount, int hotelId)
        {
            RoomTypeId = roomTypeId;
            Occupancy = occupancy;
            Amount = amount;
            CotsAvailable = cotsAvailable;
            Amenities = amenities;
            Discount = discount;
            HotelId = hotelId;
        }
    }
}
