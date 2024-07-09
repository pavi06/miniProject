namespace HotelBookingSystemAPI.Models.DTOs.HotelDTOs
{
    public class HotelRecommendationDTO
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string RoomType { get; set; }
        public double DiscountPercent { get; set; }

        public HotelRecommendationDTO(int id, string hotelName, string address, string city, string roomType, double discountPercent)
        {
            HotelId = id;
            HotelName = hotelName;
            Address = address;
            City = city;
            RoomType = roomType;
            DiscountPercent = discountPercent;
        }
    }
}
