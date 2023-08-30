using A_GroTech_Api.Data;
using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Repository
{
	public class DiscussionRepository : IDiscussionRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public DiscussionRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public bool AddDiscussionImage(int discussionId, List<ImagePostDto> image)
		{
			var discussion = _context.Discussions.Where(d => d.Id == discussionId).FirstOrDefault();
			foreach (var item in image)
			{
				var imageMap = _mapper.Map<Image>(item);
				imageMap.CreatedAt = DateTime.Now;
				imageMap.UpdatedAt = DateTime.Now;
				var discussionImage = new DiscussionImage
				{
					Discussion = discussion,
					Image = imageMap
				};
				_context.Add(discussionImage);
			}
			return Save();
		}

		public bool AddLike(int id)
		{
			var discussion = _context.Discussions.Where(d => d.Id == id).FirstOrDefault();
			discussion.Likes += 1;
			return Save();
		}

		public bool AddPinnedAnswer(int discussionId, int answerId)
		{
			var discussion = _context.Discussions.Where(d => d.Id == discussionId).FirstOrDefault();
			var answer = _context.DiscussionAnswers.Where(da => da.Id == answerId).FirstOrDefault();
			var pinned = new PinnedDiscussionAnswer
			{
				Discussion = discussion,
				DiscussionAnswer = answer
			};
			_context.Add(pinned);
			return Save();
		}

		public bool AddSolved(int id)
		{
			var discussion = _context.Discussions.Where(d => d.Id == id).FirstOrDefault();
			discussion.IsSolved = true;
			return Save();
		}

		public bool CreateDiscussion(Discussion discussion)
		{
			_context.Add(discussion);
			return Save();
		}

		public bool DeleteDiscussion(int id)
		{
			var discussion = _context.Discussions.Where(d => d.Id == id).FirstOrDefault();
			_context.Remove(discussion);
			return Save();
		}

		public Discussion GetDiscussion(int id)
		{
			var discussion = _context.Discussions
				.Where(d => d.Id == id)
				.Include(d => d.User)
				.FirstOrDefault();
			return _mapper.Map<Discussion>(discussion);
		}

		public ICollection<Image> GetDiscussionImage(int id)
		{
			var discussionImage = _context.DiscussionImages
				.Where(di => di.Discussion.Id == id)
				.Include(di => di.Image)
				.Select(di => di.Image)
				.ToList();
			return _mapper.Map<ICollection<Image>>(discussionImage);
		}

		public ICollection<Discussion> GetDiscussions(PaginationDto paginationDto)
		{
			var discussions = _context.Discussions
				.Include(d => d.User)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<Discussion>>(discussions);
		}

		public ICollection<Discussion> GetDiscussionsByUserWhoCreated(string id, PaginationDto paginationDto)
		{
			var discussions = _context.Discussions
				.Where(d => d.User.Id == id)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<Discussion>>(discussions);
		}

		public ICollection<DiscussionAnswer> GetPinnedAnswer(int id)
		{
			var pinned = _context.PinnedDiscussionAnswers
				.Where(pda => pda.Discussion.Id == id)
				.Include(pda => pda.DiscussionAnswer)
				.Include(pda => pda.DiscussionAnswer.AnsweredBy)
				.Include(pda => pda.DiscussionAnswer.Discussion)
				.Select(pda => pda.DiscussionAnswer)
				.ToList();
			return _mapper.Map<ICollection<DiscussionAnswer>>(pinned);
		}

		public bool RemoveDiscussionImage(int discussionId, int ImageId)
		{
			var discussion = _context.Discussions.Where(d => d.Id == discussionId).FirstOrDefault();
			var image = _context.Images.Where(i => i.Id == ImageId).FirstOrDefault();
			var discussionImage = _context.DiscussionImages.Where(di => di.Image.Id == image.Id && di.Discussion.Id == discussion.Id).FirstOrDefault();
			_context.Remove(discussionImage);
			_context.Remove(image);
			return Save();
		}

		public bool RemoveLike(int id)
		{
			var discussion = _context.Discussions.Where(d => d.Id == id).FirstOrDefault();
			discussion.Likes -= 1;
			return Save();
		}

		public bool RemovePinnedAnswer(int discussionId, int answerId)
		{
			var pinned = _context.PinnedDiscussionAnswers
				.Where(pda => pda.Discussion.Id == discussionId && pda.DiscussionAnswer.Id == answerId)
				.FirstOrDefault();
			_context.Remove(pinned);
			return Save();
		}

		public bool RemoveSolved(int id)
		{
			var discussion = _context.Discussions.Where(d => d.Id == id).FirstOrDefault();
			discussion.IsSolved = false;
			return Save();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public ICollection<Discussion> SearchDiscussions(string search, PaginationDto paginationDto)
		{
			var discussions = _context.Discussions
				.Where(d => d.Tittle.Contains(search) || d.Message.Contains(search))
				.Include(d => d.User)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<Discussion>>(discussions);
		}

		public bool UpdateDiscussion(Discussion discussion)
		{
			_context.Update(discussion);
			return Save();
		}
	}
}
