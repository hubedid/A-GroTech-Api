using A_GroTech_Api.Models;

namespace A_GroTech_Api.Dto.BodyModels
{
	public class NotificationPostDto
	{
		public string Message { get; set; }
		public string UserId { get; set; }
		public bool IsRead { get; set; }
	}
}
