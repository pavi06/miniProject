namespace HotelBookingSystemAPI.Models.DTOs.HotelDTOs
{
    public class UpdateHotelDTO
    {
        public int HotelId { get; set; }
        public Dictionary<string, string> AttributeValuesPair { get; set; } = new Dictionary<string, string>();

        public UpdateHotelDTO() {
        }
        public UpdateHotelDTO(int id, Dictionary<string, string> attributeValuePairs)
        {
            HotelId = id;
            AttributeValuesPair = attributeValuePairs;
        }
    }
}
