using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IDiscussionAnswerRepository
	{
		ICollection<DiscussionAnswer> GetDiscussionAnswers(PaginationDto paginationDto);
		DiscussionAnswer GetDiscussionAnswer(int id);
		ICollection<Image> GetDiscussionAnswerImages(int id);
		ICollection<DiscussionAnswer> GetDiscussionAnswersByDiscussionId(int id, PaginationDto paginationDto);
		bool AddDiscussionAnswer(DiscussionAnswer discussionAnswer);
		bool UpdateDiscussionAnswer(DiscussionAnswer discussionAnswer);
		bool DeleteDiscussionAnswer(int id);
		bool AddDiscussionAnswerLike(int id);
		bool RemoveDiscussionAnswerLike(int id);
		bool AddDiscussionAnswerImage(int discussionAnswerId, List<ImagePostDto> image);
		bool RemoveDiscussionAnswerImage(int discussionAnswerId, int ImageId);
		bool Save();
	}
}
