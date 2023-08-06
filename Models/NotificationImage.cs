namespace A_GroTech_Api.Models
{
	public class NotificationImage
	{
        public int NotificationId { get; set; }
		public int ImageId { get; set; }
		public Notification Notification { get; set; }
		public Image Image { get; set; }
    }
}
