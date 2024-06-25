namespace HotelBookingSystemAPI.Models.DTOs.HotelDTOs
{
    public class UpdateHotelDTO
    {
        public int HotelId { get; set; }
        public Dictionary<string, string> AttributeValuesPair { get; set; } = new Dictionary<string, string>();
        //public List<string> AttributeNames { get; set; }
        //public List<string> AttributeValues { get; set; }

        public UpdateHotelDTO() {
            //AttributeValuesPair = new List<(string, string)>();
        }
        public UpdateHotelDTO(int id, Dictionary<string, string> attributeValuePairs)
        {
            HotelId = id;
            AttributeValuesPair = attributeValuePairs;
        }
    }
}
