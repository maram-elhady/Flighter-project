using Microsoft.EntityFrameworkCore;

namespace Flighter.DTO.UserDto
{
    [Owned] //no primary key so i have to say this table owned by another (app user)
    public class RefreshTokens
    {

        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}
