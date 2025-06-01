namespace CompanyDashboard.Models
{
    public class UserDisplayDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class UserStatsDto
    {
        public int TotalUsers { get; set; }
        public List<UserDisplayDto> Users { get; set; } = new();
    }

}
