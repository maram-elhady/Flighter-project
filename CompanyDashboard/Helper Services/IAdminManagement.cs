using CompanyDashboard.Models;


namespace CompanyDashboard.Identity
{
    public interface IAdminManagement
    {
        Task<List<GetCompanyDto>> GetCompanies();
        Task<List<GetFlightTypesDto>> GetFlightTypes();
        Task<List<GetClassTypesDto>> GetClassTypes();
        Task<(bool isSuccess, string message, List<AdminCompanyDto> admins)> LoadAdmins();
        Task<(bool isSuccess, string message)> SubmitAdmin(string email, int companyId);
        Task<(bool isSuccess, string message)> DeleteAdmin(string email);
        Task<(bool isSuccess, string message)> SubmitFlight(AddFlightDto flightDto);
        Task<(bool isSuccess, string message, List<AdminTicketDto> tickets)> LoadTickets(string filterType = "All");
        Task<(bool isSuccess, string message)> DeleteTicket(int ticketId);
        Task<(bool isSuccess, string message, List<BookingDetailsDto> bookings)> LoadBookings();
        Task<UserStatsDto> GetAllUsersAsync();
        Task<(bool isSuccess, string message)> Deleteuser(string email);
    }
}
