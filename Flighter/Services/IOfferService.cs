using Flighter.DTO.FlightDto;
using Flighter.Helper;

namespace Flighter.Services
{
    public interface IOfferService
    {
        Task<ApiResponse<List<GetTicketOfferDto>>> GetAvailableOffersAsync();
        Task<ApiResponse<List<GetTicketOfferDto>>> GetTicketsByOfferPercentageAsync(string offerPercentage);

    }
}
