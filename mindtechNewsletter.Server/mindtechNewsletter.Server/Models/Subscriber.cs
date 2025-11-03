namespace mindtechNewsletter.Server.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true; //soft delete
    }
}
