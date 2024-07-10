namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class AvailableRoomTypesDTO
    {
        public int RoomTypeId { get; set; }
        public string RoomType { get; set; }
        public int NoOfRoomsAvailable { get; set; }
        public int Occupancy { get; set; }
        public double Amount { get; set; }
        public double? Discount { get; set; }
        public string Amenities { get; set; }
        public string Images { get; set; }

        public AvailableRoomTypesDTO(int roomtypeId,string roomType, int noOfRoomsAvailable, int occupancy, double amount,  double? discount, string amenities, string images)
        {
            RoomTypeId = roomtypeId;
            RoomType = roomType;
            NoOfRoomsAvailable = noOfRoomsAvailable;
            Occupancy = occupancy;
            Amount = amount;
            Discount = discount;
            Amenities = amenities;
            Images = images;
        }
    }
}
