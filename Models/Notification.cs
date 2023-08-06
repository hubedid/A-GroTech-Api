namespace A_GroTech_Api.Models
{
	public class Notification
	{
        public int Id { get; set; }
		public string Message { get; set; }
		public User User { get; set; }
		public bool IsRead { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
        public ICollection<NotificationImage> NotificationImages { get; set; }
    }
}
