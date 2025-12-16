namespace Core.Utilities.Security.JWT
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
