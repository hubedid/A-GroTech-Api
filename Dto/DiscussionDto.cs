using A_GroTech_Api.Models;

namespace A_GroTech_Api.Dto
{
	public class DiscussionDto
	{
		public int Id { get; set; }
        public string Tittle { get; set; }
        public string Message { get; set; }
		public int Likes { get; set; }
		public UserDto CreatedBy { get; set; }
		public bool IsSolved { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
