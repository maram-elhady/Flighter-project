using Flighter.DTO.FlightDto;
using Flighter.Helper;
using Flighter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flighter.Services
{
    public class OfferService : IOfferService
    {
        private readonly ApplicationDbContext _context;
        public OfferService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<GetTicketOfferDto>>> GetAvailableOffersAsync()
        {
            var rawTickets = await _context.tickets
                .Include(t => t.flight)
                   .ThenInclude(t => t.FlightType)
                .Include(t => t.schedule)
                .Include(t => t.Company)
                .Include(t => t.classType)
                .Where(t =>
                    t.availableSeats > 0 &&
                    t.seats.Any(fs => !fs.isBooked)&&
                    t.offer_percentage != "0" &&
                    t.offer_percentage != null &&
                    t.offer_percentage != ""
                )
                .Select(t => new
                {
                    t.TicketId,
                    t.price,
                    From = t.flight.fromLocation,
                    To = t.flight.toLocation,
                    ClassType = t.classType.className,
                    FlightType = t.flight.FlightType.FlightName,
                    DepartureDate = t.schedule.departureDate,
                    DepartureTime = t.schedule.departureTime,
                    ArrivalDate = t.schedule.arrivalDate,
                    ArrivalTime = t.schedule.arrivalTime,
                    OfferPercentage = t.offer_percentage,
                    CompanyName = t.Company.CompanyName
                })
                .ToListAsync();

            if (rawTickets == null || rawTickets.Count == 0)
            {
                return new ApiResponse<List<GetTicketOfferDto>>
                {
                    Success = false,
                    Message = "No available offers found.",
                    Data = new List<GetTicketOfferDto>()
                };
            }

            var result = rawTickets.Select(t =>
            {
                var departure = t.DepartureDate.ToDateTime(t.DepartureTime);
                var arrival = t.ArrivalDate.ToDateTime(t.ArrivalTime);

                int durationMinutes = (int)(arrival - departure).TotalMinutes;

                return new GetTicketOfferDto
                {
                    TicketId = t.TicketId,
                    Price = t.price,
                    From = t.From,
                    To = t.To,
                    classType = t.ClassType,
                    flightType = t.FlightType,
                    DepartureDate = t.DepartureDate,
                    DepartureTime = t.DepartureTime,
                    ArrivalDate = t.ArrivalDate,
                    ArrivalTime = t.ArrivalTime,
                    OfferPercentage = t.OfferPercentage,
                    DurationInMinutes = durationMinutes,
                    CompanyName = t.CompanyName
                };
            }).ToList();

            return new ApiResponse<List<GetTicketOfferDto>>
            {
                Success = true,
                Message = "Available tickets with offers retrieved successfully.",
                Data = result
            };
        }


        //filtering
        public async Task<ApiResponse<List<GetTicketOfferDto>>> GetTicketsByOfferPercentageAsync(string offerPercentage)
        {
            if (string.IsNullOrEmpty(offerPercentage))
            {
                return new ApiResponse<List<GetTicketOfferDto>>
                {
                    Success = false,
                    Message = "Offer percentage must be provided.",
                    Data = new List<GetTicketOfferDto>()
                };
            }

            if (!decimal.TryParse(offerPercentage, out decimal parsedOfferPercentage) || parsedOfferPercentage <= 0)
            {
                return new ApiResponse<List<GetTicketOfferDto>>
                {
                    Success = false,
                    Message = "Offer percentage must be a positive value.",
                    Data = new List<GetTicketOfferDto>()
                };
            }

            var rawTickets = await _context.tickets
                .Include(t => t.flight)
                   .ThenInclude(t => t.FlightType)
                .Include(t => t.schedule)
                .Include(t => t.Company)
                .Include(t => t.classType)
                .Where(t =>
                    t.offer_percentage == offerPercentage &&
                    t.availableSeats > 0 &&
                    t.seats.Any(fs => !fs.isBooked)
                )
                .Select(t => new
                {
                    t.TicketId,
                    t.price,
                    From = t.flight.fromLocation,
                    To = t.flight.toLocation,
                    ClassType = t.classType.className,
                    FlightType = t.flight.FlightType.FlightName,
                    DepartureDate = t.schedule.departureDate,
                    DepartureTime = t.schedule.departureTime,
                    ArrivalDate = t.schedule.arrivalDate,
                    ArrivalTime = t.schedule.arrivalTime,
                    OfferPercentage = t.offer_percentage,
                    CompanyName = t.Company.CompanyName
                })
                .ToListAsync();

            if (rawTickets == null || rawTickets.Count == 0)
            {
                return new ApiResponse<List<GetTicketOfferDto>>
                {
                    Success = false,
                    Message = $"No available tickets found with {offerPercentage}% offer.",
                    Data = new List<GetTicketOfferDto>()
                };
            }

            var result = rawTickets.Select(t =>
            {
                var departure = t.DepartureDate.ToDateTime(t.DepartureTime);
                var arrival = t.ArrivalDate.ToDateTime(t.ArrivalTime);
                int durationMinutes = (int)(arrival - departure).TotalMinutes;

                return new GetTicketOfferDto
                {
                    TicketId = t.TicketId,
                    Price = t.price,
                    From = t.From,
                    To = t.To,
                    classType = t.ClassType,
                    flightType = t.FlightType,
                    DepartureDate = t.DepartureDate,
                    DepartureTime = t.DepartureTime,
                    ArrivalDate = t.ArrivalDate,
                    ArrivalTime = t.ArrivalTime,
                    OfferPercentage = t.OfferPercentage,
                    DurationInMinutes = durationMinutes,
                    CompanyName = t.CompanyName
                };
            }).ToList();

            return new ApiResponse<List<GetTicketOfferDto>>
            {
                Success = true,
                Message = $"Available tickets with {offerPercentage}% offer retrieved successfully.",
                Data = result
            };
        }



    }
}