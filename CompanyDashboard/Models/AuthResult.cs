namespace CompanyDashboard.DTOs
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string[] ErrorList { get; set; } = [];
    }
}
