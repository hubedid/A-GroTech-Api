using A_GroTech_Api.Models;

namespace A_GroTech_Api.Dto
{
	public class DiscussionAnswerDto
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public int Likes { get; set; }
		public UserDto AnsweredBy { get; set; }
		public int DiscussionId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
