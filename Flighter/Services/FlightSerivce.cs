using CompanyDashboard.Pages;
using Flighter.DTO.FlightDto;
using Flighter.Helper;
using Flighter.Models;
using Humanizer;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;


namespace Flighter.Services
{
    public class FlightSerivce : IFlightService
    {
        private readonly ApplicationDbContext _context;
        public FlightSerivce(ApplicationDbContext context)
        {
            _context = context;
        }
       

        public async Task<List<GetcompanyDto>> GetAllCompaniesAsync()
        {
            return await _context.Companies
                .Select(c => new GetcompanyDto
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.CompanyName
                })
                .ToListAsync();
        }

        public async Task<List<GetFlighttypesDto>> GetAllFlightTypesAsync()
        {
            return await _context.FlightTypes
                .Select(f => new GetFlighttypesDto
                {
                    FlightTypeId = f.FlightTypeId,
                    FlightName = f.FlightName
                })
                .ToListAsync();
        }

        public async Task<List<GetClasstypesDto>> GetAllClassTypesAsync()
        {
            return await _context.classTypes
                .Select(c => new GetClasstypesDto
                {
                    classTypeId = c.classTypeId,
                    className = c.className
                })
                .ToListAsync();
        }

        public async Task<ApiResponse<List<TicketResultDto>>> SearchTicketsAsync(GetTicketDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.From) || string.IsNullOrWhiteSpace(dto.To))
            {
                return new ApiResponse<List<TicketResultDto>>
                {
                    Success = false,
                    Message = "From and To fields are required.",

                };
            }

            var ticketsQuery = _context.tickets
                            .Include(t => t.flight)
                            .Include(t => t.schedule)
                            .Include(t => t.classType)
                            .Include(t => t.Company)
                            .Include(t=>t.seats)
                            .Where(t =>
                            t.flight.fromLocation == dto.From &&
                            t.flight.toLocation == dto.To &&
                            t.flight.FlightTypeId == dto.FlightTypeId &&
                            t.classTypeId == dto.ClassTypeId &&
                           // t.availableSeats >= dto.NoOfTravelers &&
                            t.schedule.departureDate == dto.StartDate &&
                            t.offer_percentage == "0"
                           );

            // number of unbooked seats (available seats)
            ticketsQuery = ticketsQuery.Where(t =>
                _context.Flightseats
                    .Where(s => s.ticketId == t.TicketId && s.isBooked == false)
                    .Count() >= dto.NoOfTravelers
            );

            if (dto.AirlineIds != null && dto.AirlineIds.Any())
            {
                ticketsQuery = ticketsQuery.Where(t => dto.AirlineIds.Contains(t.CompanyId));
            }

            // 2️d Round-trip filter
            if (dto.FlightTypeId == 2 && dto.EndDate.HasValue)
            {
                ticketsQuery = ticketsQuery.Where(t =>
                    t.schedule.returnDepartureDate == dto.EndDate.Value);
            }

            // Project 
            var rawResults = await ticketsQuery.Select(t => new
            {
                t.TicketId,
                t.price,
                From = t.flight.fromLocation,
                To = t.flight.toLocation,
                DepartureDate = t.schedule.departureDate,
                DepartureTime = t.schedule.departureTime,
                ArrivalDate = t.schedule.arrivalDate,
                ArrivalTime = t.schedule.arrivalTime,
                CompanyName = t.Company.CompanyName
            }).ToListAsync();

            var results = rawResults.Select(t =>
            {
                var departure = t.DepartureDate.ToDateTime(t.DepartureTime);
                var arrival = t.ArrivalDate.ToDateTime(t.ArrivalTime);

                int durationMinutes = (int)(arrival - departure).TotalMinutes;

                return new TicketResultDto
                {
                    TicketId = t.TicketId,
                    Price = t.price,
                    From = t.From,
                    To = t.To,
                    DepartureDate = t.DepartureDate,
                    DepartureTime = t.DepartureTime,
                    ArrivalDate = t.ArrivalDate,
                    ArrivalTime = t.ArrivalTime,
                    DurationInMinutes = durationMinutes,
                    CompanyName = t.CompanyName
                };
            }).ToList();

            //  Sorting by price, then duration
            if (dto.FilterCheapest && dto.FilterFastest)
            {
                results = results.OrderBy(r => Convert.ToDecimal(r.Price))
                                 .ThenBy(r => r.DurationInMinutes)
                                 .ToList();
            }
            else if (dto.FilterCheapest)
            {
                results = results.OrderBy(r => Convert.ToDecimal(r.Price)).ToList();
            }
            else if (dto.FilterFastest)
            {
                results = results.OrderBy(r => r.DurationInMinutes).ToList();
            }


            return new ApiResponse<List<TicketResultDto>>
            {
                Success = true,
                Data = results,

            };
        }


        public async Task<ApiResponse<List<string>>> GetFromLocationsAsync()
        {
            var fromLocations = await _context.flights
                .Select(f => f.fromLocation)
                .Distinct()
                .OrderBy(f => f)
                .ToListAsync();

            return new ApiResponse<List<string>>
            {
                Success = true,
                Data = fromLocations,

            };
        }

        public async Task<ApiResponse<List<string>>> GetToLocationsAsync()
        {
            var toLocations = await _context.flights
                .Select(f => f.toLocation)
                .Distinct()
                .OrderBy(f => f)
                .ToListAsync();

            return new ApiResponse<List<string>>
            {
                Success = true,
                Data = toLocations,

            };
        }

        public async Task<ApiResponse<SeatSelectionDto>> SeatsStatusAsync(int ticket_id)
        {
            // Fetch the ticket and seats associated with the ticketId
            var ticket = await _context.tickets
                .Include(t => t.seats)
                .FirstOrDefaultAsync(t => t.TicketId == ticket_id);

            if (ticket == null)
            {
                return new ApiResponse<SeatSelectionDto>()
                {
                    Success = true,
                    Message = "Ticket is not found"

                };
            }

            var seats = await _context.Flightseats
                     .Where(s => s.ticketId == ticket_id)
                     .Select(s => new SeatDto
                     {
                        SeatId = s.SeatId,
                        seatName = s.SeatName,
                        IsBooked = s.isBooked
                     })
                     .ToListAsync();

            var responseDto = new SeatSelectionDto
            {
                TicketId = ticket_id,
                Seats = seats
            };

            return new ApiResponse<SeatSelectionDto>()
            {
                Success = true,
                Data = responseDto

            };
        }

        public async Task<ApiResponse<TicketSummaryDto>> GetTicketSummaryAsync(TicketSummaryRequestDto request)
        {
            if (request?.SelectedSeats == null)
                return new ApiResponse<TicketSummaryDto> { Success = false, Message = "Invalid payload" };

            var ticket = await _context.tickets
                .Include(t => t.flight)
                .Include(t => t.schedule)
                .Include(t => t.classType)
                .FirstOrDefaultAsync(t => t.TicketId == request.TicketId);

            if (ticket == null)
                return new ApiResponse<TicketSummaryDto> { Success = false, Message = "Ticket not found" };

            var firstSeat = request.SelectedSeats.FirstOrDefault() ?? "NA";
            var summary = new TicketSummaryDto
            {
                From = ticket.flight.fromLocation,
                To = ticket.flight.toLocation,
                DepartureDate = ticket.schedule.departureDate,
                DepartureTime = ticket.schedule.departureTime,
                FlightNumber = ticket.flightNumber,
                Gate = ticket.gate,
                ClassName = ticket.classType.className,
                SelectedSeats = request.SelectedSeats,
                TicketCode = $"{ticket.flightNumber}-{ticket.gate}-{firstSeat}"
            };

            return new ApiResponse<TicketSummaryDto> { Success = true, Data = summary };
        }

        public async Task<ApiResponse<List<UserBookingDto>>> GetUserBookingsAsync(string userid)
        {
            var username = await _context.Users
                .Where(u => u.Id == userid)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            if (username == null)
            {
                return new ApiResponse<List<UserBookingDto>>
                {
                    Success = false,
                    Message = "No user found."
                };
            }

            var bookings = await _context.bookings
                .Where(b => b.userId == userid)
                .Select(b => new { b.BookingId, b.bookingDate, b.ticketId, b.paymentStatus })
                .ToListAsync();

            if (!bookings.Any())
            {
                return new ApiResponse<List<UserBookingDto>>
                {
                    Success = false,
                    Message = "No bookings found for the user."
                };
            }

            var userBookings = new List<UserBookingDto>();

            foreach (var booking in bookings)
            {
                var ticket = await _context.tickets
                    .Where(t => t.TicketId == booking.ticketId)
                    .Include(t => t.schedule)
                    .Include(t => t.seats)
                    .Include(t => t.flight)
                    .Include(t => t.classType)
                    .Include(t => t.Company)
                    .FirstOrDefaultAsync();

                if (ticket == null) continue;

                
                var bookedSeatIds = await _context.bookingSeats
                    .Where(bs => bs.BookingId == booking.BookingId)
                    .Select(bs => bs.SeatId)
                    .ToListAsync();

               
                var seatNames = await _context.Flightseats
                    .Where(s => bookedSeatIds.Contains(s.SeatId))
                    .Select(s => s.SeatName)
                    .ToListAsync();

                
                decimal finalAmount;

                if (booking.paymentStatus == "Paid")
                {
                    // Parse string amount to decimal
                    var amountString = await _context.payments
                        .Where(p => p.BookingId == booking.BookingId)
                        .Select(p => p.Amount)
                        .FirstOrDefaultAsync();

                    var amountMatch = Regex.Match(amountString ?? "", @"^\d+");
                    finalAmount = amountMatch.Success ? decimal.Parse(amountMatch.Value) : 0;
                }
                else
                {
                    // Parse ticket price string to decimal
                    var priceMatch = Regex.Match(ticket.price ?? "", @"^\d+");
                    decimal priceValue = priceMatch.Success ? decimal.Parse(priceMatch.Value) : 0;
                    finalAmount = priceValue * bookedSeatIds.Count;
                }
                var firstSeat = seatNames.FirstOrDefault() ?? "NA";
                var departureDateTime = ticket.schedule.departureDate.ToDateTime(ticket.schedule.departureTime);
                var arrivalDateTime = ticket.schedule.arrivalDate.ToDateTime(ticket.schedule.arrivalTime);
                int durationMinutes = (int)(arrivalDateTime - departureDateTime).TotalMinutes;

                userBookings.Add(new UserBookingDto
                {
                    Guestname = username,
                    CompanyName = ticket.Company.CompanyName,
                    From = ticket.flight.fromLocation,
                    To = ticket.flight.toLocation,
                    DepartureDate = ticket.schedule.departureDate,
                    DepartureTime = ticket.schedule.departureTime,
                    ArrivalDate = ticket.schedule.arrivalDate,
                    ArrivalTime = ticket.schedule.arrivalTime,
                    ReturnDepartureDate = ticket.schedule.returnDepartureDate,
                    ReturnDepartureTime = ticket.schedule.returnDepartureTime,
                    ReturnArrivalDate = ticket.schedule.arrivalDate,
                    ReturnArrivalTime = ticket.schedule.arrivalTime,
                    ClassName = ticket.classType.className,
                    FlightNumber = ticket.flightNumber,
                    Gate = ticket.gate,
                    TicketCode = $"{ticket.flightNumber}-{ticket.gate}-{firstSeat}",
                    BaggageAllowance = ticket.BaggageAllowance,
                    SelectedSeats = seatNames,
                    BookingDate = booking.bookingDate,
                    PaymentStatus = booking.paymentStatus,
                    amount = finalAmount,
                    bookingid = booking.BookingId,
                    ticketid=booking.ticketId,
                    DurationInMinutes = durationMinutes
                });
            }

            return new ApiResponse<List<UserBookingDto>>
            {
                Success = true,
                Data = userBookings,
                Message = "User bookings returned successfully."
            };
        }



    }
}
