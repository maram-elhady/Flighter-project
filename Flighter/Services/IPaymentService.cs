using Flighter.DTO;
using Flighter.DTO.FlightDto;
using Flighter.Helper;

namespace Flighter.Services
{
    public interface IPaymentService
    {
        Task<ApiResponse<int>> CreateBookingAsync(PayDto request);
        Task<ApiResponse<string>> PaymenthistoryAsync(PayhistoryDto paydto);
        Task<ApiResponse<RefundDto>> RefundAsync(int bookingid);
        Task RemoveExpiredPayLaterAsync();
        Task RemoveExpiredPayPendingAsync();

    }
}
