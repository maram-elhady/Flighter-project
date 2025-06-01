using CompanyDashboard.Identity;
using CompanyDashboard.Models;
using Microsoft.AspNetCore.Components;

namespace CompanyDashboard.Pages
{
    public partial class Tickets : ComponentBase
    {

        //intialize variables
        private AddFlightDto flightDto = new AddFlightDto();
        private bool showFlightForm = false;
        private int currentStep = 1;
        private bool showErrors = false;
        private string ErrorMessage = string.Empty;
        private int TotalSeats = 24;
        private string selectedFilterType = "Filtering";
        private List<Seat> Seats = new();
        private HashSet<string> SelectedSeats = new();
        private List<GetFlightTypesDto> FlightTypes = new();
        private List<GetClassTypesDto> ClassTypes = new();
        private string selectedFlightTypeName = "Select Flight Type";
        private string selectedClassTypeName = "Select Class Type";
        private List<AddFlightDto> flightTickets = new();
        private string? responseMessage;
        private string alertClass = "alert-info";
        private int roundTripId = 2;
        private List<AdminTicketDto> tickets = new();
        private bool isOwner = false;
        private bool isLoading = true;
        private Timer? _timer;


        protected override async Task OnInitializedAsync()
        {
            var cookieProvider = AccountManagement as CookieAuthenticationStateProvider;
            if (cookieProvider != null)
            {
                var authState = await cookieProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                isOwner = user.Identity != null && user.Identity.Name == "flighter924@gmail.com";
            }
            flightDto.offer_percentage = "0";
            FlightTypes = await AdminManagement.GetFlightTypes();
            ClassTypes = await AdminManagement.GetClassTypes();
            await LoadTicketsData();
            // Poll every 10 seconds
            _timer = new Timer(async _ =>
            {
                await InvokeAsync(async () =>
                {
                    await LoadTicketsData();
                    StateHasChanged();
                });
            }, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
           // isLoading = false;
            //StateHasChanged();
        }

       
        private async Task SelectFilterType(string type)
        {
            selectedFilterType = type;
            await LoadTicketsData(type);
        }


        private async Task LoadTicketsData(string filterType = "All")
        {
            isLoading = true;
            var (isSuccess, message, ticketList) = await AdminManagement.LoadTickets(filterType);
            isLoading = false;
            if (!isSuccess)
            {
                await ShowMessage(message, "alert-danger");
            }

            tickets = ticketList;
            StateHasChanged();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
        //view ticket details 
        int? showDetailsId = null;
        void ToggleDetails(int ticketId)
        {
            if (showDetailsId == ticketId)
                showDetailsId = null;
            else
                showDetailsId = ticketId;
        }



        private void ValidateSeats()
        {
            Console.WriteLine($"Class: {flightDto.ClassTypeId}, Seats: {flightDto.AvailableSeats}");
            if (flightDto.AvailableSeats < 1 || (flightDto.AvailableSeats > 16 && flightDto.ClassTypeId == 1)
            || (flightDto.AvailableSeats > 28 && flightDto.ClassTypeId == 2))
            {
                ErrorMessage = "You have to enter a number between 1 and 16 if bussiness , 28 otherwise.";
                showErrors = true;
            }
            else
            {
                ErrorMessage = string.Empty;
                if (flightDto.ClassTypeId == 1)
                {
                    GenerateBussinessSeat();
                }
                else if (flightDto.ClassTypeId == 2)
                {
                    GenerateEconomySeats();
                }
            }
        }


        private void NextStep()
        {
            showErrors = true; 


            if (currentStep == 1)
            {
                flightDto.DepartureDate = flightDto.DepartureDate == default ? DateOnly.FromDateTime(DateTime.UtcNow) : flightDto.DepartureDate;
                flightDto.ArrivalDate = flightDto.ArrivalDate == default ? DateOnly.FromDateTime(DateTime.UtcNow) : flightDto.ArrivalDate;

                if (string.IsNullOrWhiteSpace(flightDto.From) ||
                    string.IsNullOrWhiteSpace(flightDto.To) ||
                    flightDto.FlightTypeId == null || flightDto.FlightTypeId == 0)
                {
                    return; // Stop here if any required field is empty
                }
            }


            if (currentStep == 2)
            {
                // Validate Departure and Arrival Date and Time
                if (flightDto.DepartureDate == default || flightDto.ArrivalDate == default)
                {
                    ErrorMessage = "Please set both departure and arrival dates.";
                    return;
                }

                // Direct flight validation
                if (flightDto.FlightTypeId != roundTripId)
                {
                    if (flightDto.DepartureDate > flightDto.ArrivalDate)
                    {
                        ErrorMessage = "Departure date cannot be after arrival date.";
                        return;
                    }

                    if (flightDto.DepartureDate == flightDto.ArrivalDate)
                    {
                        if (flightDto.DepartureTime == flightDto.ArrivalTime)
                        {
                            ErrorMessage = "Departure time cannot be the same as arrival time on the same day.";
                            return;
                        }

                        if (flightDto.DepartureTime > flightDto.ArrivalTime)
                        {
                            ErrorMessage = "Departure time must be earlier than arrival time on the same day.";
                            return;
                        }
                    }
                }

                // Round trip validation
                if (flightDto.FlightTypeId == roundTripId)
                {
                    if (flightDto.DepartureDate > flightDto.ArrivalDate)
                    {
                        ErrorMessage = "Departure date cannot be after arrival date.";
                        return;
                    }

                    if (flightDto.DepartureDate == flightDto.ArrivalDate &&
                        flightDto.DepartureTime >= flightDto.ArrivalTime)
                    {
                        ErrorMessage = "Departure time must be before arrival time on the same day.";
                        return;
                    }



                    // Validate return dates

                    if (flightDto.returnDepartureDate == default && flightDto.FlightTypeId == 2)
                    {
                        return;
                    }
                    if (flightDto.returnDepartureTime == default && flightDto.FlightTypeId == 2)
                    {
                        return;
                    }
                    if (flightDto.returnArrivalDate == default && flightDto.FlightTypeId == 2)
                    {
                        return;
                    }
                    if (flightDto.returnArrivalTime == default && flightDto.FlightTypeId == 2)
                    {
                        return;
                    }
                    if (flightDto.returnDepartureDate < flightDto.ArrivalDate)
                    {
                        ErrorMessage = "Return departure date must be on or after the original arrival date.";
                        return;
                    }

                    if (flightDto.returnDepartureDate == flightDto.ArrivalDate &&
                        flightDto.returnDepartureTime <= flightDto.ArrivalTime)
                    {
                        ErrorMessage = "Return departure time must be after original arrival time on the same day.";
                        return;
                    }

                    if (flightDto.returnArrivalDate < flightDto.returnDepartureDate)
                    {
                        ErrorMessage = "Return arrival date must be on or after return departure date.";
                        return;
                    }

                    if (flightDto.returnArrivalDate == flightDto.returnDepartureDate &&
                        flightDto.returnArrivalTime <= flightDto.returnDepartureTime)
                    {
                        ErrorMessage = "Return arrival time must be after return departure time on the same day.";
                        return;
                    }
                }

            }



            else if (currentStep == 3)
            {
                ValidateSeats();
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(flightDto.FlightName) ||
                    string.IsNullOrWhiteSpace(flightDto.Gate) ||
                    flightDto.AvailableSeats < 1 ||
                    string.IsNullOrWhiteSpace(flightDto.Price) ||
                    flightDto.ClassTypeId == null || flightDto.ClassTypeId == 0 || string.IsNullOrWhiteSpace(flightDto.BaggageAllowance) || !int.TryParse(flightDto.BaggageAllowance, out int baggage) ||
                    baggage > 50)
                {
                    return;
                }
            }



            showErrors = false; // Hide errors once all fields are valid
            ErrorMessage = null;
            currentStep++; // Move to the next step
        }


        private void GenerateBussinessSeat()
        {
            Seats.Clear();
            string[] rows = { "A", "B", "C", "D" };
            for (int row = 1; row <= 4; row++)
            {
                foreach (var col in rows)
                {
                    Seats.Add(new Seat { SeatLabel = $"{row}{col}", IsAvailable = true });
                }
            }
            if (TotalSeats == flightDto.AvailableSeats)
            {
                foreach (var seat in Seats)
                {
                    seat.IsAvailable = false;
                    SelectedSeats.Add(seat.SeatLabel);
                }
            }
        }
        private void GenerateEconomySeats()
        {
            Seats.Clear();
            string[] rows = { "E", "F", "G", "H" };
            for (int row = 1; row <= 7; row++)
            {
                foreach (var col in rows)
                {
                    Seats.Add(new Seat { SeatLabel = $"{row}{col}", IsAvailable = true });
                }
            }
            if (TotalSeats == flightDto.AvailableSeats)
            {
                foreach (var seat in Seats)
                {
                    seat.IsAvailable = false;
                    SelectedSeats.Add(seat.SeatLabel);
                }
            }
        }
        void ToggleSeat(Seat seat)
        {
            if (!seat.IsAvailable)
                return;

            if (SelectedSeats.Contains(seat.SeatLabel))
            {
                SelectedSeats.Remove(seat.SeatLabel); 
            }
            else if (SelectedSeats.Count < flightDto.AvailableSeats)
            {
                SelectedSeats.Add(seat.SeatLabel); 
            }
        }


       

        //Add Ticket
        private async Task SubmitFlight()
        {
            flightDto.SeatNames = SelectedSeats.ToList();
            var (isSuccess, message) = await AdminManagement.SubmitFlight(flightDto);

            if (isSuccess)
            {
                flightTickets.Add(flightDto);
                showFlightForm = false;
                flightDto = new AddFlightDto();
                flightDto.offer_percentage = "0"; 
                selectedFlightTypeName = "Select Flight Type";
                selectedClassTypeName = "Select Class Type";
                currentStep = 1;
                StateHasChanged();
            }

            await ShowMessage(message, isSuccess ? "alert-success" : "alert-danger");
           

        }


        //Delete Ticket
        private bool showDeleteModal = false;
        private int? ticketIdToDelete = null;
        private void ConfirmDelete(int ticketId)
        {
            ticketIdToDelete = ticketId;
            showDeleteModal = true;
        }

        private void CancelDelete()
        {
            ticketIdToDelete = null;
            showDeleteModal = false;
        }

        private async Task DeleteConfirmed()
        {
            if (ticketIdToDelete.HasValue)
            {
                var (isSuccess, message) = await AdminManagement.DeleteTicket(ticketIdToDelete.Value);
                if (isSuccess)
                {
                    tickets = tickets.Where(t => t.TicketId != ticketIdToDelete.Value).ToList();
                }

                ticketIdToDelete = null;
                showDeleteModal = false;

                await ShowMessage(message, isSuccess ? "alert-success" : "alert-danger");
            }

        }


        private void SelectFlightType(GetFlightTypesDto flightType)
        {
            selectedFlightTypeName = flightType.FlightName;
            flightDto.FlightTypeId = flightType.FlightTypeId;
        }

        private void SelectClassType(GetClassTypesDto classType)
        {
            selectedClassTypeName = classType.className;
            flightDto.ClassTypeId = classType.classTypeId;
            // Reset seats and seat count
            SelectedSeats.Clear();
            flightDto.AvailableSeats = 0;
        }
        private async Task ShowMessage(string message, string alertType)
        {
            responseMessage = message;
            alertClass = alertType;
            StateHasChanged();
            await Task.Delay(3000);
            responseMessage = null;
            StateHasChanged();
        }

        private void ShowForm()
        {
            showFlightForm = true;
            currentStep = 1;

            // Reset the flight data
            flightDto = new AddFlightDto
            {
                offer_percentage = "0",
                DepartureDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ArrivalDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            // Reset selections
            SelectedSeats.Clear();
            Seats.Clear();
            selectedFlightTypeName = "Select Flight Type";
            selectedClassTypeName = "Select Class Type";

            // Reset validation and errors
            showErrors = false;
            ErrorMessage = string.Empty;
        }
        private void HideForm() => showFlightForm = false;
        private void PreviousStep()
        {
            if (currentStep == 4)
            {
                SelectedSeats.Clear(); 
            }
            currentStep--;
        }

        public class Seat
        {
            public string SeatLabel { get; set; } = string.Empty;
            public bool IsAvailable { get; set; } = true;
        }
    }
}
