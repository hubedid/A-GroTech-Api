﻿using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace A_GroTech_Api.Interfaces
{
	public interface IDiscussionAnswerRepository
	{
		ICollection<DiscussionAnswer> GetDiscussionAnswers();
		DiscussionAnswer GetDiscussionAnswer(int id);
		ICollection<Image> GetDiscussionAnswerImages(int id);
		ICollection<DiscussionAnswer> GetDiscussionAnswersByDiscussionId(int id);
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