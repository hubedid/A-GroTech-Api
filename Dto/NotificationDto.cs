using A_GroTech_Api.Models;

namespace A_GroTech_Api.Dto
{
	public class NotificationDto
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public UserDto User { get; set; }
		public bool IsRead { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
