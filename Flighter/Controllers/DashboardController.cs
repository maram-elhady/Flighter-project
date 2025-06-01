using CompanyDashboard.Models;
using Flighter.Models;
using Flighter.Models.Dashboard;
using Flighter.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CompanyDashboard.DTOs;
using Flighter.Models.DBModels;
using Flighter.DTO.FlightDto;
using Flighter.DTO.UserDto;
using CompanyDashboard.Pages;

namespace Flighter.Controllers
{
    [Route("Dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dasboardService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public DashboardController(UserManager<ApplicationUser> userManager, IDashboardService dasboardService, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _dasboardService = dasboardService;
            _context = context;
            _signInManager = signInManager;
        }

        //[HttpGet("login")]
        //public IActionResult Index()
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Html", "login.html");
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return NotFound("Login page not found.");
        //    }
        //    return PhysicalFile(filePath, "text/html");
        //}

        //[Authorize(Policy = "SpecificUserOnly")]
        //[HttpGet("index")]
        //public IActionResult GetIndexPage()
        //{
        //    return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Html", "index.html"), "text/html");
        //}

        //[HttpPost("Home")]
        //public async Task<IActionResult> Login([FromBody] loginModel loginData)
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Html", "index.html");
        //    // Example validation (this should be replaced with actual validation logic)
        //    if (loginData.Email == "admin@example.com" && loginData.Password == "password")
        //    {
        //        // Redirect to the dashboard (or any other route after successful login)
        //        return Ok(new { RedirectUrl = "/Html/index.html" }/*new { message = "Login successful" }*/);
        //    }
        //    else
        //    {
        //        // Return unauthorized if credentials are incorrect
        //        return Unauthorized(new { message = "Invalid credentials" });
        //    }
        //}

        //[Authorize(Policy = "SpecificUserOnly")]
        //[HttpGet("addAdmin")]
        //public IActionResult GetAddAdminPage()
        //{
        //    return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Html", "addAdmin.html"), "text/html");
        //}




        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var companyId = int.TryParse(User.FindFirstValue("CompanyId"), out var id) ? id : 0;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {

                return Unauthorized(new { message = "User not authenticated" });
            }



            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new UserInfo
            {
                Email = user.Email!,
                Role = role,
                CompanyId = companyId
            });
        }


        [HttpPost("admin-login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles == null || !userRoles.Any())
            {
                return Forbid();
            }

            var userRole = userRoles.FirstOrDefault();

            if (userRole?.Equals("Company", StringComparison.OrdinalIgnoreCase) == true ||
                userRole?.Equals("Owner", StringComparison.OrdinalIgnoreCase) == true)

            {
                var claims = new List<Claim>
            {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, userRole),
            new Claim("CompanyId", user.CompanyId.ToString())
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                };



                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                return Ok(new { Message = "Login successful", Role = userRole });

            }
            return Forbid();
        }
        
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] object empty)
        {
            if (empty is not null)
            {
                await _signInManager.SignOutAsync();
                Response.Cookies.Delete(".AspNetCore.Cookies");//for publish (delete auth cookies)
                return Ok();
            }

            return Unauthorized();
        }


        [Authorize(Roles = "Owner")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("add-admin")]
        public async Task<IActionResult> AddAdminAsync([FromBody] AddRoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _dasboardService.AddAdminAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(new { Message = "Admin assigned to company and role successfully!" });
        }

        
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("get-All-Admins")]
        public async Task<IActionResult> GetAdminsAsync()
        {
            var admins = await _context.Users
                .Where(u => u.CompanyId != null)
                .Select(u => new
                {
                    Email = u.Email,
                    Company = u.Company.CompanyName
                })
                .ToListAsync();

            if (admins == null || !admins.Any())
                return NotFound(new { Message = "No admins found" });

            return Ok(admins);
        }

        [Authorize(Roles = "Owner")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpDelete("delete-Admins/{email}")]
        public async Task<IActionResult> DeleteAsync(string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _dasboardService.DeleteRoleAsync(email);

            if (result == "Admin doesn't exist")
                return NotFound(result);

            if (result.StartsWith("Exception happened") || result.StartsWith("Database error"))
                return StatusCode(500, result);

            return Ok(new { Message = result });
        }

        [Authorize(Roles = "Company")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpDelete("Ticket/{ticketId}")]
        public async Task<IActionResult> DeleteTicket(int ticketId)
        {
            // Extract the user's email from the claims
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User email claim not found. Ensure you are sending a valid token.");
            }

            var admin = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (admin == null)
            {
                return Unauthorized("Admin not found.");
            }

            try
            {
                var ticket = await _context.tickets
                    .Include(t => t.flight)
                    .FirstOrDefaultAsync(t => t.TicketId == ticketId && t.CompanyId == admin.CompanyId);

                if (ticket == null)
                {
                    return NotFound("Ticket not found");
                }
                var flight = ticket.flight;

                // Delete the ticket
                _context.tickets.Remove(ticket);
                await _context.SaveChangesAsync();

                // Check if this was the only ticket linked to the flight
                bool otherTicketsExist = await _context.tickets
                    .AnyAsync(t => t.flightId == flight.FlightId && t.TicketId != ticketId);

                if (!otherTicketsExist)
                {
                    _context.flights.Remove(flight);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { Message = "Ticket deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [Authorize(Roles = "Company")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("Ticket")]
        public async Task<IActionResult> AddFlight([FromBody] AddFlightDto flightDto)
        {

            if (flightDto == null || !flightDto.SeatNames.Any())
                return BadRequest("Invalid flight data or no seats provided.");

            // Get the current user's email from the claims
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User email claim not found. Ensure you are sending a valid token.");
            }

            // Find the admin user in the database
            var admin = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (admin == null)
                return Unauthorized("Admin not found.");

            // Ensure the admin is assigned to a company
            if (admin.CompanyId == null)
                return BadRequest("Admin is not assigned to a company.");


            var fromLocation = CapitalizeFirstLetter(flightDto.From);
            var toLocation = CapitalizeFirstLetter(flightDto.To);

            // Create and add the new flight
            var newFlight = new FlightModel
            {
                fromLocation = fromLocation,
                toLocation = toLocation,
                CompanyId = admin.CompanyId.Value,
                FlightTypeId = flightDto.FlightTypeId
            };

            _context.flights.Add(newFlight);
            await _context.SaveChangesAsync();

            // Create and add the new schedule
            var newSchedule = new ScheduleModel
            {
                flightId = newFlight.FlightId,
                departureDate = flightDto.DepartureDate,
                departureTime = flightDto.DepartureTime,
                arrivalDate = flightDto.ArrivalDate,
                arrivalTime = flightDto.ArrivalTime,
                returnDepartureDate = flightDto.returnDepartureDate,
                returnDepartureTime = flightDto.returnDepartureTime,
                returnArrivalDate = flightDto.returnArrivalDate,
                returnArrivalTime = flightDto.returnArrivalTime
            };

            _context.schedules.Add(newSchedule);
            await _context.SaveChangesAsync();

            // Create and add the new ticket
            var newTicket = new TicketModel
            {
                flightId = newFlight.FlightId,
                classTypeId = flightDto.ClassTypeId,
                scheduleId = newSchedule.ScheduleId,
                price = flightDto.Price,
                gate = flightDto.Gate,
                flightNumber = flightDto.FlightName,
                offer_percentage = flightDto.offer_percentage,
                totalSeats = 24,
                availableSeats = flightDto.AvailableSeats,
                userId = admin.Id,
                CompanyId = admin.CompanyId.Value,
                BaggageAllowance = flightDto.BaggageAllowance + " KG"
            };

            _context.tickets.Add(newTicket);
            await _context.SaveChangesAsync();

            // Add seats for the ticket
            var seatEntities = flightDto.SeatNames.Select(seatName => new FlightSeatModel
            {
                ticketId = newTicket.TicketId,
                SeatName = seatName,
                isBooked = false
            }).ToList();

            _context.Flightseats.AddRange(seatEntities);
            await _context.SaveChangesAsync();

            return Ok("Flight added successfully.");


        }

        
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("Ticket")]
        public async Task<IActionResult> GetFlight([FromQuery] string? filterType = "All")
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail)) return Unauthorized("User email claim not found.");

            var currentUser = await _userManager.FindByEmailAsync(userEmail);
            if (currentUser == null) return Unauthorized("User not found.");

            var isOwner = await _userManager.IsInRoleAsync(currentUser, "Owner");

            IQueryable<TicketModel> ticketQuery = _context.tickets
                .Include(t => t.flight).ThenInclude(f => f.FlightType)
                .Include(t => t.schedule)
                .Include(t => t.classType)
                .Include(t => t.seats)
                .Include(t => t.Company);

            if (!isOwner)
            {
                ticketQuery = ticketQuery.Where(t => t.Admin.Id == currentUser.Id);
            }

            if (filterType == "With Offers")
            {
                ticketQuery = ticketQuery.Where(t => t.offer_percentage != null && t.offer_percentage != "0");
            }
            else if (filterType == "Without Offers")
            {
                ticketQuery = ticketQuery.Where(t => string.IsNullOrEmpty(t.offer_percentage) || t.offer_percentage == "0");
            }

            var ticketList = await ticketQuery.ToListAsync();

            var tickets = ticketList.Select(t =>
            {
                // Combine DateOnly and TimeOnly into DateTime
                var departure = t.schedule.departureDate.ToDateTime(t.schedule.departureTime);
                var arrival = t.schedule.arrivalDate.ToDateTime(t.schedule.arrivalTime);
                int duration = (int)(arrival - departure).TotalMinutes;

                return new AdminTicketDto
                {
                    TicketId = t.TicketId,
                    FlightNumber = t.flightNumber,
                    Gate = t.gate,
                    Price = t.price,
                    OfferPercentage = t.offer_percentage,
                    AvailableSeats = t.availableSeats,
                    TotalSeats = t.totalSeats,
                    BaggageAllowance = t.BaggageAllowance,
                    ClassType = t.classType.className,
                    Company = new AdminTicketDto.CompanyInfo
                    {
                        CompanyName = t.Company.CompanyName,
                    },
                    Flight = new AdminTicketDto.FlightInfo
                    {
                        FlightId = t.flight.FlightId,
                        FromLocation = t.flight.fromLocation,
                        ToLocation = t.flight.toLocation,
                        FlightType = t.flight.FlightType.FlightName
                    },
                    Schedule = new AdminTicketDto.ScheduleInfo
                    {
                        DepartureDate = t.schedule.departureDate,
                        DepartureTime = t.schedule.departureTime,
                        ArrivalDate = t.schedule.arrivalDate,
                        ArrivalTime = t.schedule.arrivalTime,
                        ReturnDepartureDate = t.schedule.returnDepartureDate,
                        ReturnDepartureTime = t.schedule.returnDepartureTime,
                        ReturnArrivalDate = t.schedule.returnArrivalDate,
                        ReturnArrivalTime = t.schedule.returnArrivalTime,
                        DurationInMinutes = duration 
                    },
                    SeatNames = t.seats.Select(s => new AdminTicketDto.SeatInfo
                    {
                        SeatName = s.SeatName,
                        IsBooked = s.isBooked
                    }).ToList()
                };
            }).ToList();

            return Ok(tickets);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookings()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var bookings = await _dasboardService.GetAdminBookingsAsync(email);

            return Ok(bookings);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("users/all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userData = users.Select(u => new
            {
                u.Email,
                u.Name
            }).ToList();

            var response = new
            {
                TotalUsers = userData.Count,
                //OnlineUsers = new Random().Next(1, userData.Count),
                Users = userData
            };

            return Ok(response);
        }
        [Authorize(Roles = "Owner")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpDelete("user/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var users = await _userManager.Users.ToListAsync();

            var userEmail = users.FirstOrDefault(u => u.Email == email);
            // Delete the ticket
            _context.Users.Remove(userEmail);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User deleted successfully" });
        }
        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
