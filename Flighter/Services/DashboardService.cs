using CompanyDashboard.Models;
using Flighter.DTO.UserDto;
using Flighter.Models;
using Flighter.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flighter.Services
{
    public class DashboardService : IDashboardService
    {
        // private readonly HttpClient _httpClient;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public DashboardService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }


        public async Task<string> AddAdminAsync(AddRoleDto model)
        {
            
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return "Invalid email. User not found.";

            if (user.CompanyId != null)
                return "User is already assigned to a company.";

            //var company = await _context.Companies.FindAsync(model.CompanyId);
            //if (company == null)
            //    return "Invalid company selection.";

            user.CompanyId = model.CompanyId;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return "Failed to assign company.";

            //var companyRole = await _roleManager.FindByNameAsync("Company");
            //if (companyRole == null)
            //    return "Company role does not exist in the system.";

            var roleResult = await _userManager.AddToRoleAsync(user, "Company");
            if (!roleResult.Succeeded)
                return "Failed to assign company role.";

            return string.Empty; 
        }


        public async Task<string> DeleteRoleAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return "Admin doesn't exist";
            }

            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.Id);
            if (userRole == null)
            {
                return "User is not assigned to any company role";
            }

            try
            {
                
                _context.UserRoles.Remove(userRole);
                user.CompanyId = null;
                _context.Users.Update(user);


                
                var role = await _roleManager.FindByNameAsync("Company");
                if (role != null && await _userManager.IsInRoleAsync(user, "Company"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Company");
                }

                await _context.SaveChangesAsync();

                return "Admin Removed Successfully";
            }
            catch (DbUpdateException dbEx)
            {
                return $"Database error: {dbEx.Message}";
            }
            catch (Exception ex)
            {
                return $"Exception happened: {ex.Message}";
            }
        }

        
        public async Task<List<BookingDetailsDto>> GetAdminBookingsAsync(string adminEmail)
        {
            var admin = await _context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
            if (admin == null) return new List<BookingDetailsDto>();

            // Get role name 
            var roleName = await (from ur in _context.UserRoles
                                  join r in _context.Roles on ur.RoleId equals r.Id
                                  where ur.UserId.ToString() == admin.Id 
                                  select r.Name)
                                  .FirstOrDefaultAsync();

            
            var query = _context.bookings
                .Include(b => b.ticket)
                    .ThenInclude(t => t.schedule)
                .Include(b => b.ticket)
                    .ThenInclude(t => t.flight)
                    .ThenInclude(t=>t.Company)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .Include(b => b.user)
                .AsQueryable();

            
            if (!string.Equals(roleName, "Owner", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(b => b.ticket.CompanyId == admin.CompanyId);
            }

            // Projection
            var bookings = await query
                .Select(b => new BookingDetailsDto
                {
                    ticketid=b.ticket.TicketId,
                    FlightNumber = b.ticket.flightNumber,
                    userEmail = b.user.Email,
                    BookingDate = b.bookingDate,
                    PaymentStatus = b.paymentStatus,
                    DepartureDate = b.ticket.schedule.departureDate,
                    CompanyName= b.ticket.Company.CompanyName ,
                    ReservedSeats = b.BookingSeats.Select(bs => bs.Seat.SeatName).ToList()
                })
                .ToListAsync();

            return bookings;
        }






    }
}
