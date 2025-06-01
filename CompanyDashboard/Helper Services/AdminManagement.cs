using System.Net.Http.Json;
using System.Text.Json;
using CompanyDashboard.Models;
using static System.Net.WebRequestMethods;


namespace CompanyDashboard.Identity
{
    public class AdminManagement(IHttpClientFactory httpClientFactory) : IAdminManagement
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Auth");

        public async Task<List<GetCompanyDto>> GetCompanies()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<GetCompanyDto>>("flight/companies") ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading companies: {ex.Message}");
                return new List<GetCompanyDto>(); 
            }
        }


        public async Task<List<GetFlightTypesDto>> GetFlightTypes()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<GetFlightTypesDto>>("flight/FlightTypes") ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading companies: {ex.Message}");
                return new List<GetFlightTypesDto>();
            }
        }

        public async Task<List<GetClassTypesDto>> GetClassTypes()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<GetClassTypesDto>>("flight/ClassTypes") ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading companies: {ex.Message}");
                return new List<GetClassTypesDto>();
            }
        }


        public async Task<(bool isSuccess, string message, List<AdminCompanyDto> admins)> LoadAdmins()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<AdminCompanyDto>>("Dashboard/get-All-Admins");
                return (true, "Admins loaded successfully", response ?? new List<AdminCompanyDto>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching admins: {ex.Message}");
                return (false, $"Error fetching admins: {ex.Message}", new List<AdminCompanyDto>());
            }
        }



        public async Task<(bool isSuccess, string message)> SubmitAdmin(string email, int companyId)
        {
            if (string.IsNullOrWhiteSpace(email) || companyId == 0 )
            {
                return (false, "Please fill in all fields.");
            }

            var model = new AddAdminCompanyDto
            {
                Email = email,
                CompanyId = companyId
            };

            var response = await _httpClient.PostAsJsonAsync("Dashboard/add-admin", model);
            var message = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return (true, "Role added successfully");
            }
            else
            {
                return (false, $"Error: {message}");
            }
        }


        public async Task<(bool isSuccess, string message)> DeleteAdmin(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "Please enter an email.");
            }

            var response = await _httpClient.DeleteAsync($"Dashboard/delete-Admins/{email}");
            var message = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return (true, "Admin deleted successfully");
            }
            else
            {
                return (false, $"Error: {message}");
            }
        }

        public async Task<(bool isSuccess, string message)> SubmitFlight(AddFlightDto flightDto)
        {
            if (flightDto is null)
            {
                return (false, "Please fill in all fields.");
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("Dashboard/Ticket", flightDto);
                var message = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Flight added successfully");
                }
                else
                {
                    return (false, $"Error: {message}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Exception: {ex.Message}");
            }
        }


        public async Task<(bool isSuccess, string message, List<AdminTicketDto> tickets)> LoadTickets(string filterType = "All")
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<AdminTicketDto>>($"Dashboard/Ticket?filterType={filterType}");
                return (true, "Tickets loaded successfully", response ?? new List<AdminTicketDto>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching tickets: {ex.Message}");
                return (false, $"Error fetching tickets: {ex.Message}", new List<AdminTicketDto>());
            }
        }
        public async Task<(bool isSuccess, string message)> DeleteTicket(int ticketId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Dashboard/Ticket/{ticketId}");
                var message = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Ticket deleted successfully");
                }
                else
                {
                    return (false, $"Error: {message}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Exception: {ex.Message}");
            }
        }


        public async Task<(bool isSuccess, string message, List<BookingDetailsDto> bookings)> LoadBookings()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<BookingDetailsDto>>($"Dashboard/bookings");
                return (true, "Bookings loaded successfully", response ?? new List<BookingDetailsDto>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching bookings: {ex.Message}");
                return (false, $"Error fetching bookings: {ex.Message}", new List<BookingDetailsDto>());
            }
        }

        public async Task<UserStatsDto> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("Dashboard/users/all");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserStatsDto>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            })!;
        }
        public async Task<(bool isSuccess, string message)> Deleteuser(string email)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Dashboard/user/{email}");
                var message = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (true, "user deleted successfully");
                }
                else
                {
                    return (false, $"Error: {message}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Exception: {ex.Message}");
            }
        }

    }
}
