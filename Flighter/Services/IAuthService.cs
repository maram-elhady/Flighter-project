using Flighter.DTO.UserDto;
using Flighter.Models;
using Microsoft.AspNetCore.Identity;

namespace Flighter.Services
{
    public interface IAuthService
    {
        Task <string>RegisterAsync(RegisterModelDto model);
        Task<AuthModel> GetTokenAsync(TokenRequestDto model);
        

        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<AuthModel> VerifyEmailAsync(string email, string verificationCode);
        Task<string> SendResetPasswordCodeAsync(string email);
        Task<string> VerifyResetCodeInternal(string email, string code); 
        Task<string> ResetPasswordAsync(string email,string newPassword);
        Task<string> UpdateProfileAsync(ApplicationUser user/*string email*/, string? name, string? profilePhoto, DateTime? dateOfBirth, string country);
       Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword);
       Task<string> DeleteProfilePhotoAsync(ApplicationUser user);
       Task<EditUserProfileDto> GetUserProfileAsync(ApplicationUser user);
        
    }
}
