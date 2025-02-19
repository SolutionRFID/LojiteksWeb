namespace LojiteksWeb.Models
{
    public class EmailConfirmedModel
    {
        public bool EmailConfirmed { get; set; }
        public DateTime? TwoFactorCodeExpiration { get; set; }
    }
}
