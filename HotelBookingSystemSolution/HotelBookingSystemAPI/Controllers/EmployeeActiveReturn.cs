using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;

namespace HotelBookingSystemAPI.Controllers
{
    internal class EmployeeActiveReturn : IRepository<int, HotelEmployee>
    {
        private string v;

        public EmployeeActiveReturn(string v)
        {
            this.v = v;
        }
    }
}