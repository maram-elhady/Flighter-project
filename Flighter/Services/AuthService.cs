using Flighter.Helper;
using Flighter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Http.HttpResults;
using Flighter.DTO.UserDto;

namespace Flighter.Services
{

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IPasswordHasher<RegisterModelDto> _passwordHasher;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager, IPasswordHasher<RegisterModelDto> passwordHasher, IMemoryCache cache, IConfiguration configuration)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _cache = cache;
            _configuration = configuration;
        }


        public async Task<string> RegisterAsync(RegisterModelDto model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return "Email is already registered!";

            var passwordValidationResult = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, null, model.Password);
            //if (!passwordValidationResult.Succeeded)
            //{
            //    var errors = string.Join(", ", passwordValidationResult.Errors.Select(e => e.Description));
            //    return $"Password is not strong enough: {errors}";
            //}
            foreach (var validator in _userManager.PasswordValidators)
            {
                var result = await validator.ValidateAsync(_userManager, null, model.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return $"Password is not strong enough: {errors}";
                }
            }

            // Generate a random verification code
            var verificationCode = new Random().Next(1000, 9999).ToString();
            model.VerificationCode = verificationCode;

            // Retrieve SMTP settings from configuration
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            // Send email with verification code
            using (var smtpClient = new SmtpClient(smtpSettings["Server"]))
            {
                smtpClient.Port = 587;//int.Parse(smtpSettings["Port"]);
                smtpClient.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]),
                    Subject = "Verify your email",
                    Body = $@"
                   <html>
                   <body>
                   <p>Dear User,</p>
                   <p>Thank you for using <strong>Flighter</strong>.</p>
                    <p>Your email verification code is: <span style='color:red; font-weight:bold;'>{verificationCode}</span></p>
                    <p>Please use this code to verify your email address.</p>
                    <p>This code is valid for the next 10 minutes.</p>
                    <p>Best regards,</p>
                    <p><strong>Flighter Team</strong></p>
                    </body>
                   </html>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(model.Email);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    return "Failed to send verification email. Please try again.";
                }
            }

            // Save the unverified user to an in-memory store (e.g., a cache) with a timeout
            _cache.Set(model.Email, model, TimeSpan.FromMinutes(10));  // Adjust expiration as needed

            return "Verification email sent. Please check your inbox.";
        }
        public async Task<AuthModel> VerifyEmailAsync(string email, string verificationCode)
        {
            if (!_cache.TryGetValue(email, out RegisterModelDto model))
                return new AuthModel { Message = "Invalid or expired verification request." };

            if (model.VerificationCode != verificationCode)
                return new AuthModel { Message = "Incorrect verification code." };

            // If verification succeeds, create the user
            var user = new ApplicationUser
            {
                UserName = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                ConfirmPassword = _passwordHasher.HashPassword(null, model.ConfirmPassword),
                // PhoneNumber="none"
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");
            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                // ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
                UserId=user.Id,
            };
        }

        public async Task<AuthModel> GetTokenAsync(TokenRequestDto model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                 return authModel;
                 //return authModel(500, authModel.Message);
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            // authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            authModel.UserId = user.Id;

            //check if end user has active tokens
            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshTokens = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }

            //has no active token
            else
            {
                var refreshToken = GenerateRfreshToken();
                authModel.RefreshTokens = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken); //store in memory
                await _userManager.UpdateAsync(user); //store in database
            }

            return authModel;
        }


        
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issue,
                audience: _jwt.Audeince,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        private RefreshTokens GenerateRfreshToken() //generate refresh token
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshTokens
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(20), //after 20 days it will expire
                CreatedOn = DateTime.UtcNow,
            };
        }




        public async Task<string> SendResetPasswordCodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "Email not found.";
            }

            // Generate a random reset password code and the token from UserManager
            var resetPasswordCode = new Random().Next(1000, 9999).ToString();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Store the code and the token in the cache
            _cache.Set(email + "_resetCode", new { Code = resetPasswordCode, Token = token }, TimeSpan.FromMinutes(10));

            // Send email with reset pass code
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            using (var smtpClient = new SmtpClient(smtpSettings["Server"]))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]),
                    Subject = "Reset Password Code",
                    Body = $@"
                <html>
                    <body>
                        <p>Dear User,</p>
                        <p>Thank you for using <strong>Flighter</strong>.</p>
                        <p>Your reset password code is: <span style='color:red; font-weight:bold;'>{resetPasswordCode}</span></p>
                        <p>Please use this code to reset your password.</p>
                        <p>This code is valid for the next 10 minutes.</p>
                        <p>Best regards,</p>
                        <p><strong>Flighter Team</strong></p>
                    </body>
                </html>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception)
                {
                    return "Failed to send reset password email. Please try again.";
                }
            }

            return "Reset password code sent. Please check your inbox.";
        }

        public async Task<string> VerifyResetCodeInternal(string email, string code)
        {
            if (!_cache.TryGetValue(email + "_resetCode", out dynamic resetData) || resetData.Code != code)
            {
                return "Invalid or expired reset password code.";
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "User not found.";
            }

            var token = resetData.Token;
            var isValidToken = await _userManager.VerifyUserTokenAsync(user,
                _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);

            if (!isValidToken)
            {
                return "Invalid or expired token.";
            }

            // Store verified email in a user-specific cache key
            _cache.Set($"verifiedEmail_{email}", email, TimeSpan.FromMinutes(10));

            return "Email and code verified successfully.";
        }


        public async Task<string> ResetPasswordAsync(string email, string newPassword)
        {
            if (!_cache.TryGetValue($"verifiedEmail_{email}", out string cachedEmail) || cachedEmail != email)
            {
                return "Session expired or email not verified. Please verify your email and code again.";
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "User not found.";
            }

            if (!_cache.TryGetValue(email + "_resetCode", out dynamic resetData))
            {
                return "Invalid or expired reset code.";
            }

            foreach (var validator in _userManager.PasswordValidators)
            {
                var validationResult = await validator.ValidateAsync(_userManager, user, newPassword);
                if (!validationResult.Succeeded)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.Description));
                    return $"New password does not meet security requirements: {errors}";
                }
            }

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetData.Token, newPassword);
            if (!resetPasswordResult.Succeeded)
            {
                var errorsList = new List<string>();

                foreach (var error in resetPasswordResult.Errors)
                {
                    errorsList.Add(error.Description);
                }

                var errors = string.Join(", ", errorsList);

                return $"Failed to reset password: {errors}";
            }

            _cache.Remove(email + "_resetCode");
            _cache.Remove($"verifiedEmail_{email}");

            return "Password has been reset successfully.";
        }



        public async Task<AuthModel> RefreshTokenAsync(string token)
        {
            var authModel = new AuthModel();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                // authModel.IsAuthenticated = false;
                authModel.Message = "invalid token";
                return authModel;
            }
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
            {
                // authModel.IsAuthenticated = false;
                authModel.Message = "inactive token";
                return authModel;
            }

            //only used once then revoke
            refreshToken.RevokedOn = DateTime.UtcNow;
            //add new one
            var newRefreshToken = GenerateRfreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            //new JWT
            var jwtToken = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshTokens = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authModel;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                return false;
            }
            //there is already refresh token exist with user
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
            {
                return false;
            }

            //only used once then revoke
            refreshToken.RevokedOn = DateTime.UtcNow;


            await _userManager.UpdateAsync(user);

            return true;
        }


        public async Task<string> UpdateProfileAsync(ApplicationUser user/*string email*/,string? name, string? profilePhoto, DateTime? dateOfBirth, string country)
        {
            //var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return "User not found.";

            // Update the fields if provided
            if (!string.IsNullOrEmpty(name))
                user.Name = name;

            user.ProfilePhoto = profilePhoto ?? user.ProfilePhoto;
            user.DateOfBirth = dateOfBirth ?? user.DateOfBirth;
            user.Country = country ?? user.Country;

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return "Profile updated successfully.";
        }
        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword)
        {
            // Check if the old password is correct
            var isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isOldPasswordCorrect)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Old password is incorrect." });
            }

            //password is strong
            var passwordValidationResult = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, user, newPassword);
            if (!passwordValidationResult.Succeeded)
            {
                var errors = string.Join(", ", passwordValidationResult.Errors.Select(e => e.Description));
                return IdentityResult.Failed(new IdentityError { Description = $"New password is not strong enough: {errors}" });
            }


            // Change the password
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<string> DeleteProfilePhotoAsync(ApplicationUser user)
        {
            if (user == null)
                return "User not found.";

            if (string.IsNullOrEmpty(user.ProfilePhoto))
                return "No profile photo to delete.";

            // Get the full path of the photo
            var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePhoto.TrimStart('/'));

            // Remove the photo from the file system if it exists
            if (File.Exists(photoPath))
            {
                File.Delete(photoPath);
            }

            // Clear the ProfilePhoto field in the database
            user.ProfilePhoto = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return "Profile photo deleted successfully.";
        }

        public async Task<EditUserProfileDto> GetUserProfileAsync(ApplicationUser user)
        {
            if (user == null)
                new AuthModel { Message = "User not found." };



            var userProfile = new EditUserProfileDto
            {
                Name = user.Name,
                DateOfBirth = user.DateOfBirth,
                Country = user.Country
            };

            return userProfile;
        }

      
    }
}
