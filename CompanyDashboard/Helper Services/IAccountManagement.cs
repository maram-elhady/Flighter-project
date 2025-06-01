using CompanyDashboard.DTOs;
using CompanyDashboard.Models;
using static CompanyDashboard.Pages.Company;

namespace CompanyDashboard.Identity
{
    public interface IAccountManagement
    {

        Task<AuthResult> LoginAsync(LoginModel credentials);
        Task LogoutAsync();

        //Task<List<AdminCompany>> LoadAdmins();
    }
}
