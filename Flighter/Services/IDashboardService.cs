using CompanyDashboard.Models;
using Flighter.DTO.UserDto;
using Flighter.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace Flighter.Services
{
    public interface IDashboardService
    {
      // Task<loginModel> FetchDataAsync();
       Task<string> AddAdminAsync(AddRoleDto model);
       Task<string> DeleteRoleAsync(string email);
       Task<List<BookingDetailsDto>> GetAdminBookingsAsync(string adminEmail);

    }
}
