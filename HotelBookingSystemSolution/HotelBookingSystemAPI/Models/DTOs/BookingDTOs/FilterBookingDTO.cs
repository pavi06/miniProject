namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class FilterBookingDTO
    {
        public string Attribute { get; set; }
        public string AttributeValue { get; set; }

        public FilterBookingDTO(string attribute, string attributeValue) { 
            Attribute = attribute;
            AttributeValue = attributeValue;
        }
    }
}
