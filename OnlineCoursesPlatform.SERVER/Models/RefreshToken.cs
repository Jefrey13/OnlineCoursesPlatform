namespace OnlineCoursesPlatform.SERVER.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsRevoked { get; set; }

        // Relación con el usuario
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
