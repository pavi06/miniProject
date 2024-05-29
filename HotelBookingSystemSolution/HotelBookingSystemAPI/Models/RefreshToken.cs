namespace HotelBookingSystemAPI.Models
{
    public class RefreshToken
    {
        public string RfrshToken { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime ExpiresOn { get; set; }
    }
}
