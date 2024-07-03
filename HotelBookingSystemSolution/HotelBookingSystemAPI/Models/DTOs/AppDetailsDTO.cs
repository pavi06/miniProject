namespace HotelBookingSystemAPI.Models.DTOs
{
    public class AppDetailsDTO
    {
        public int TotalNoOfHotelsAvailable { get; set; }
        public int TotalNoOfUsers { get; set; }
        public int TotalNoOfEmployees { get; set; }
        public double AverageBookingPerMonth { get; set; }
        public double AverageUsageTime { get; set; } = 3.5;

        public AppDetailsDTO(int totalHotels, int totalUsers, int totalEmployees , double AverageBookings) {
            TotalNoOfHotelsAvailable = totalHotels;
            TotalNoOfUsers = totalUsers;
            TotalNoOfEmployees = totalEmployees;
            AverageBookingPerMonth = AverageBookings;
        }
    }
}
