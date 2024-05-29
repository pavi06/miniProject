namespace HotelBookingSystemAPI.Models.DTOs.EmployeeDTOs
{
    public class EmployeeRegisterReturnDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }

        public EmployeeRegisterReturnDTO(string name, string email, string phoneNumber, string address, string role)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            Role = role;
        }
    }
}
