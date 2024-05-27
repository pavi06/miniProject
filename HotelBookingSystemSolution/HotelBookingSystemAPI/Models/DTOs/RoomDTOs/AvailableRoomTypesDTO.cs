namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class AvailableRoomTypesDTO
    {
        public string RoomType { get; set; }
        public int NoOfRoomsAvailable { get; set; }
        public int Occupancy { get; set; }
        public double Amount { get; set; }
        public int CotsAvailable { get; set; }
        public string Amenities { get; set; }
        public double? Discount { get; set; } 

        public AvailableRoomTypesDTO(string roomType, int noOfRoomsAvailable, int occupancy, double amount, int cotsAvailable, string amenities, double? discount)
        {
            RoomType = roomType;
            NoOfRoomsAvailable = noOfRoomsAvailable;
            Occupancy = occupancy;
            Amount = amount;
            CotsAvailable = cotsAvailable;
            Amenities = amenities;
            Discount = discount;
        }
    }
}
