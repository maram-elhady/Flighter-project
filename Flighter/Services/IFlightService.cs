using CompanyDashboard.Models;
using Flighter.DTO.FlightDto;
using Flighter.Helper;
using Microsoft.EntityFrameworkCore;

namespace Flighter.Services
{
    public interface IFlightService
    {
        Task<List<GetcompanyDto>> GetAllCompaniesAsync();
        Task<List<GetFlighttypesDto>> GetAllFlightTypesAsync();
        Task<List<GetClasstypesDto>> GetAllClassTypesAsync();
        Task<ApiResponse<List<TicketResultDto>>> SearchTicketsAsync(GetTicketDto dto);
        Task<ApiResponse<List<string>>> GetFromLocationsAsync();
        Task<ApiResponse<List<string>>> GetToLocationsAsync();
        Task<ApiResponse<SeatSelectionDto>> SeatsStatusAsync(int ticket_id);
        Task<ApiResponse<TicketSummaryDto>> GetTicketSummaryAsync(TicketSummaryRequestDto request);
        Task<ApiResponse<List<UserBookingDto>>> GetUserBookingsAsync(string userid);
        
    }
}
