namespace HotelBookingSystemAPI.Models.DTOs.HotelDTOs
{
    public class UpdateHotelDTO
    {
        public int HotelId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }

        public UpdateHotelDTO() { }
        public UpdateHotelDTO(int id, string attributeName, string attributeValue)
        {
            HotelId = id;
            AttributeName = attributeName;
            AttributeValue = attributeValue;
        }
    }
}
