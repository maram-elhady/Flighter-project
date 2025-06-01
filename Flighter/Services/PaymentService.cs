using Flighter.DTO;
using Flighter.DTO.FlightDto;
using Flighter.Helper;
using Flighter.Models;
using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Flighter.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<int>> CreateBookingAsync(PayDto paydto)
        {
            //check seats and  reserve it
            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            var seats = await _context.Flightseats
                .Where(s => paydto.SeatsId.Contains(s.SeatId) && s.ticketId == paydto.TicketId)
                .ToListAsync();

            if (seats.Count != paydto.SeatsId.Count || seats.Any(s => s.isBooked))
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = "One or more selected seats are already booked"
                };
            }


            //  Create booking
            var booking = new BookingModel
            {
                userId = paydto.UserId,
                ticketId = paydto.TicketId,
                bookingDate = DateTime.UtcNow,
                paymentStatus = paydto.PayNow ? "Pending" : "Pay Later"
            };

            await _context.bookings.AddAsync(booking);
            await _context.SaveChangesAsync();


            foreach (var seat in seats)
            {
                seat.isBooked = true;
                seat.UserId = paydto.UserId;

                var bookingSeat = new BookingSeatModel
                {
                    BookingId = booking.BookingId,
                    SeatId = seat.SeatId
                };

                await _context.bookingSeats.AddAsync(bookingSeat);
            }
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return new ApiResponse<int>
            {
                Success = true,
                Data= booking.BookingId,
                Message = paydto.PayNow
            ? "Booking done successfully and Proceed to payment."
            : "Booking done successfully with Pay Later."
            };
        }


        public async Task<ApiResponse<string>> PaymenthistoryAsync(PayhistoryDto paydto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var booking = await _context.bookings.FirstOrDefaultAsync(b => b.BookingId == paydto.BookingId && b.paymentStatus != "Paid");

            if (booking == null || (DateTime.UtcNow - booking.bookingDate).TotalDays > 5)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "This Booking doesn't exist."
                };
            }

            var Payment = new PaymentModel
            {
                BookingId = paydto.BookingId,
                Payment_Intent_Id = paydto.PaymentIntentId,
                Amount = paydto.Amount
            };

            _context.payments.Add(Payment);
            await _context.SaveChangesAsync();


            booking.paymentStatus = "Paid";
            _context.bookings.Update(booking);


            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Payment completed successfully."
            };
        }


        public async Task<ApiResponse<RefundDto>> RefundAsync(int bookingid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var booking = await _context.bookings
                     .Where(b => b.BookingId == bookingid && b.paymentStatus == "Paid")
                     .FirstOrDefaultAsync();


            if (booking == null)
            {
                return new ApiResponse<RefundDto>
                {
                    Success = false,
                    Message = "This Payment history doesn't exist."
                };
            }

            if ((DateTime.UtcNow - booking.bookingDate).TotalDays > 2)
            {
                return new ApiResponse<RefundDto>
                {
                    Success = false,
                    Message = "Refund not allowed. Booking was made more than 2 days ago."
                };
            }

            
            var payment = await _context.payments
                       .FirstOrDefaultAsync(p => p.BookingId == bookingid);

            if (payment == null)
            {
                return new ApiResponse<RefundDto>
                {
                    Success = false,
                    Message = "No payment record found for this booking."
                };
            }

            var refundDto = new RefundDto
            {
                PaymentintentId = payment.Payment_Intent_Id,
                Amount = payment.Amount
            };


            var bookingSeats = await _context.bookingSeats
                .Where(bs => bs.BookingId == bookingid)
                .ToListAsync();

            foreach (var bookingSeat in bookingSeats)
            {
                var seat = await _context.Flightseats
                    .FirstOrDefaultAsync(fs => fs.SeatId == bookingSeat.SeatId);

                if (seat != null)
                {
                    seat.isBooked = false; // Mark seat as available again
                    seat.UserId=null;
                    _context.Flightseats.Update(seat);
                }

                
                _context.bookingSeats.Remove(bookingSeat);
            }
             _context.payments.Remove(payment);
            _context.bookings.Remove(booking);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new ApiResponse<RefundDto>
            {
                Success = true,
                Data=refundDto,
                Message = "Refund Done successfully."
            };
        }

        public async Task RemoveExpiredPayLaterAsync()
        {
            var expirationDate = DateTime.UtcNow.AddHours(-120);

            // Get expired bookings with related BookingSeats and Seats
            var expiredBookings = await _context.bookings
                .Where(b => b.paymentStatus == "Pay Later" && b.bookingDate <= expirationDate)
                .Include(b => b.BookingSeats) // Includes bookingSeats list
                    .ThenInclude(bs => bs.Seat)
                .Include(b => b.payment)
                .ToListAsync();

            if (!expiredBookings.Any()) return;

            // Collect all seatIds to reset
            var seatIdsToReset = expiredBookings
                .SelectMany(b => b.BookingSeats)
                .Select(bs => bs.SeatId)
                .Distinct()
                .ToList();

            var seatsToUpdate = await _context.Flightseats
                .Where(s => seatIdsToReset.Contains(s.SeatId))
                .ToListAsync();

            foreach (var seat in seatsToUpdate)
            {
                seat.isBooked = false;
                seat.UserId = null;
            }

            // Delete BookingSeats
            var bookingSeatEntries = expiredBookings.SelectMany(b => b.BookingSeats).ToList();
            _context.bookingSeats.RemoveRange(bookingSeatEntries);

            // Delete Payments
            var paymentsToDelete = expiredBookings.Where(b => b.payment != null).Select(b => b.payment).ToList();
            _context.payments.RemoveRange(paymentsToDelete);

            //  delete Bookings
            _context.bookings.RemoveRange(expiredBookings);

            await _context.SaveChangesAsync();
        }


        public async Task RemoveExpiredPayPendingAsync()
        {
            var expirationDate = DateTime.UtcNow.AddMinutes(-15);

            // Get expired bookings with related BookingSeats and Seats
            var expiredBookings = await _context.bookings
                .Where(b => b.paymentStatus == "Pending" && b.bookingDate <= expirationDate)
                .Include(b => b.BookingSeats) // Includes bookingSeats list
                    .ThenInclude(bs => bs.Seat)
                .Include(b => b.payment)
                .ToListAsync();

            if (!expiredBookings.Any()) return;

            // Collect all seatIds to reset
            var seatIdsToReset = expiredBookings
                .SelectMany(b => b.BookingSeats)
                .Select(bs => bs.SeatId)
                .Distinct()
                .ToList();

            var seatsToUpdate = await _context.Flightseats
                .Where(s => seatIdsToReset.Contains(s.SeatId))
                .ToListAsync();

            foreach (var seat in seatsToUpdate)
            {
                seat.isBooked = false;
                seat.UserId = null;
            }

            // Delete BookingSeats
            var bookingSeatEntries = expiredBookings.SelectMany(b => b.BookingSeats).ToList();
            _context.bookingSeats.RemoveRange(bookingSeatEntries);

            // Delete Payments
            var paymentsToDelete = expiredBookings.Where(b => b.payment != null).Select(b => b.payment).ToList();
            _context.payments.RemoveRange(paymentsToDelete);

            //  delete Bookings
            _context.bookings.RemoveRange(expiredBookings);

            await _context.SaveChangesAsync();
        }





    }

}
