using System.ComponentModel.DataAnnotations;

namespace A_GroTech_Api.Dto.BodyModels
{
	public class DiscussionAnswerPostDto
	{
		[Required]
		public string Message { get; set; }
		[Required]
		public string CreatedById { get; set; }
	}
}
