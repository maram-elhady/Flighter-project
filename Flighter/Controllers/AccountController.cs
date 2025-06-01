using Flighter.DTO.UserDto;
using Flighter.Models;
using Flighter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Flighter.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
       private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AccountController(IAuthService authService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _authService = authService;
            _userManager = userManager;
            _context = context;

        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModelDto model)
        {

            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return BadRequest(new { Message = ModelState });
            }

            var result = await _authService.RegisterAsync(model);


            if (result == "Email is already registered!")
                return BadRequest(new { Message = result });

            if (result == "Failed to send verification email. Please try again.")
                return StatusCode(500, new { Message = result });

            if (result.StartsWith("Password is not strong enough"))
                return StatusCode(400, new { Message = result });

            //if (result.StartsWith("Verification email sent.Please check your inbox."))
                //return StatusCode(200, new { Message = result });

            return StatusCode(200, new { Message = result });
            //return Ok(result);
            //return Ok(new {token=result.Token,expiresOn=result.ExpiresOn});
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmailAsync([FromBody] VerificationDto model)
        {
            var result = await _authService.VerifyEmailAsync(model.Email, model.VerificationCode);

            //SetRefreshTokenInCookie(result.RefreshTokens, result.RefreshTokenExpiration);

            if (result.ToString().StartsWith("Invalid or expired verification request."))
                return StatusCode(400, new { Message = result.Message });

            if (result.ToString().StartsWith ( "Incorrect verification code."))
                return StatusCode(400, new { Message = result.Message });

            if (!result.IsAuthenticated)
            {
                // return BadRequest(result.Message);
                return StatusCode(400, new { Message = result.Message });
            }

            //if (result.RefreshTokens != null && result.RefreshTokenExpiration != null)
            //{
            //    SetRefreshTokenInCookie(result.RefreshTokens, result.RefreshTokenExpiration);
            //}
            // return Ok(result);
            return StatusCode(200, new { Message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestDto model)
        {
            if (!ModelState.IsValid)
                 return BadRequest(new { Message = ModelState });

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
            {
                // return BadRequest(result/*.Message*/);
                return StatusCode(400, new { Message = result.Message});
            }
            
            // Check if the RefreshTokens is not empty or null, and then set the cookie
            if (!string.IsNullOrEmpty(result.RefreshTokens))
            {
                SetRefreshTokenInCookie(result.RefreshTokens, result.RefreshTokenExpiration);
            }
            Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
            {
                HttpOnly = true,  // Prevent JavaScript access
                Secure = true,    // Send only over HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF attacks
                Expires = DateTime.UtcNow.AddHours(1) // Set expiration time
            });

            return StatusCode(200, new { Message = result });
        }
        


        [HttpPost("send-reset-password-code")]
        public async Task<IActionResult> SendResetPasswordCodeAsync([FromBody] SendResetPasswordCodeDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = ModelState });

            var result = await _authService.SendResetPasswordCodeAsync(model.Email);
            if (result.StartsWith("Failed") || result.StartsWith("Email not found"))
                return BadRequest(new { Message = result });

            return Ok(new { Message = result });
        }
        [HttpPost("verify-resetPass-code")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = ModelState });
            }
            var result = await _authService.VerifyResetCodeInternal(model.Email, model.Code);
            if (result == "Email and code verified successfully.")
            {
                return Ok(new { Message = result });
            }

            return BadRequest(new { Error = result });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = ModelState });
            }

            var result = await _authService.ResetPasswordAsync(model.Email, model.NewPassword);
            if (result == "Password has been reset successfully.")
            {
                return Ok(new { Message = result });
            }

            return BadRequest(new { Error = result });
        }


        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RefreshTokenAsync(refreshToken);
            //invalid or inactive token
            if(!result.IsAuthenticated)
                return BadRequest(new { Error = result });

            SetRefreshTokenInCookie(result.RefreshTokens, result.RefreshTokenExpiration);
            //return Ok(result);
            return Ok(new { Message = result });

        }
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Invalid request" });
            } 

            var token = model.Token ??/*if token null do */ Request.Cookies["refreshToken"];

            //true if empty
            if (string.IsNullOrEmpty(token))
            {
                return StatusCode(400, new { Message = "token is required" });
            }

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
            {
                
                return StatusCode(400, new { Message = "token is invalid"});
            }

            return Ok(new { Message = "Token revoked successfully" });

        }
        //token added to cookies with the response
        private void SetRefreshTokenInCookie(string refreshToken,DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken",refreshToken, cookieOptions);
        }


        
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfileAsync([FromForm] EditUserProfileDto model)
        {
            

            var userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
                return StatusCode(401, new { Message = "User not authenticated." });

            // Find the user using the UserManager
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return StatusCode(400, new { Message = "User not found." });

            string profilePhotoPath = user.ProfilePhoto;
            
            // Handle file upload if a file is provided
            if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
            {
                // Save the file to the server
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile_photos");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{model.ProfilePhoto.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePhoto.CopyToAsync(stream);
                }

                profilePhotoPath = $"/profile_photos/{fileName}";
            }

            
            var result = await _authService.UpdateProfileAsync(user, model.Name, profilePhotoPath, model.DateOfBirth, model.Country);

            if (result.StartsWith("Profile updated"))
                return Ok(new { Message = result });

            return BadRequest(new { Message = result });
        }
       // [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto model)
        {
            var userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
            {  return StatusCode(401, new { Message = "User not authenticated." });
               // return Unauthorized( "User not authenticated." );
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return StatusCode(400, new { Message = "User not found." });

            // Check if the new password and confirm password match
            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest("New password and confirmation password do not match.");
            }

            var result = await _authService.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(new { Message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            return Ok(new { Message = "Password changed successfully." });
        }
        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccountAsync([FromBody] DeleteAccountDto model)
        {
            var userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
                return StatusCode(401, new { Message = "User not authenticated." });

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return StatusCode(400, new { Message = "User not found." });

            // Verify the old password
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordCorrect)
            {
                //return BadRequest("Password is incorrect.");
                return StatusCode(400, new { Message = "Password is incorrect." });
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }

            return Ok(new { Message = "Account deleted successfully." });
        }



        [HttpDelete("delete-profile-photo")]
        public async Task<IActionResult> DeleteProfilePhotoAsync()
        {
            var userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                // return Unauthorized("User not authenticated.");
                return StatusCode(401, new { Message = "User not authenticated." });
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // return BadRequest("User not found.");
                return StatusCode(400, new { Message = "User not found." });
            }
            // Call the AuthService to delete the profile photo
            var result = await _authService.DeleteProfilePhotoAsync(user);

            if (result == "Profile photo deleted successfully.")
            {
                 return Ok(new { Message = result });
            }
            return BadRequest(new { Message = result });
        }

        [HttpGet("get-profile-photo")]
        public async Task<IActionResult> GetProfilePhotoFileAsync()
        {
           
            var userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(401, new { Message = "User not authenticated." });
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return StatusCode(400, new { Message = "User not found." });
            }
            if (string.IsNullOrEmpty(user.ProfilePhoto))
            {
                return StatusCode(400, new { Message = "No profile photo available." });
            }
           // Get the physical file path
            var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePhoto.TrimStart('/'));

            if (!System.IO.File.Exists(photoPath))
                return NotFound("Profile photo file not found.");

            var contentType = "image/jpeg"; 
            return PhysicalFile(photoPath, contentType);
        }

        [HttpGet("get-user-profile")]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            
            var userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
                return StatusCode(401, new { Message = "User not authenticated." });


            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return StatusCode(400, new { Message = "User not found." });


            var result = await _authService.GetUserProfileAsync(user);

            
            return Ok( new { result.Name, result.DateOfBirth, result.Country });
        }



        

    }

}
