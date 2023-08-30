using A_GroTech_Api.Data;
using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Repository
{
	public class DiscussionAnswerRepository : IDiscussionAnswerRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public DiscussionAnswerRepository(DataContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		public bool AddDiscussionAnswer(DiscussionAnswer discussionAnswer)
		{
			_context.Add(discussionAnswer);
			return Save();
		}

		public bool AddDiscussionAnswerImage(int discussionAnswerId, List<ImagePostDto> image)
		{
			var discussion = _context.DiscussionAnswers.Where(d => d.Id == discussionAnswerId).FirstOrDefault();
			foreach (var item in image)
			{
				var imageMap = _mapper.Map<Image>(item);
				imageMap.CreatedAt = DateTime.Now;
				imageMap.UpdatedAt = DateTime.Now;
				var discussionAnswerImage = new DiscussionAnswerImage
				{
					DiscussionAnswer = discussion,
					Image = imageMap
				};
				_context.Add(discussionAnswerImage);
			}
			return Save();
		}

		public bool AddDiscussionAnswerLike(int id)
		{
			var discussionAnswer = _context.DiscussionAnswers.Where(d => d.Id == id).FirstOrDefault();
			discussionAnswer.Likes += 1;
			return Save();
		}

		public bool DeleteDiscussionAnswer(int id)
		{
			var discussionAnswer = _context.DiscussionAnswers.Find(id);
			_context.Remove(discussionAnswer);
			return Save();
		}

		public DiscussionAnswer GetDiscussionAnswer(int id)
		{
			var discussionAnswer = _context.DiscussionAnswers
				.Where(da => da.Id == id)
				.Include(da => da.AnsweredBy)
				.Include(da => da.Discussion)
				.FirstOrDefault();
			return _mapper.Map<DiscussionAnswer>(discussionAnswer);
		}

		public ICollection<Image> GetDiscussionAnswerImages(int id)
		{
			var discussionImage = _context.DiscussionAnswerImages
				.Where(di => di.DiscussionAnswer.Id == id)
				.Include(di => di.Image)
				.Select(di => di.Image)
				.ToList();
			return _mapper.Map<ICollection<Image>>(discussionImage);
		}

		public ICollection<DiscussionAnswer> GetDiscussionAnswers(PaginationDto paginationDto)
		{
			var discussionAnswers = _context.DiscussionAnswers
				.Include(da => da.AnsweredBy)
				.Include(da => da.Discussion)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<DiscussionAnswer>>(discussionAnswers);
		}

		public ICollection<DiscussionAnswer> GetDiscussionAnswersByDiscussionId(int id, PaginationDto paginationDto)
		{
			var discussionAnswers = _context.DiscussionAnswers
				.Where(da => da.Discussion.Id == id)
				.Include(da => da.AnsweredBy)
				.Include(da => da.Discussion)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<DiscussionAnswer>>(discussionAnswers);
		}

		public bool RemoveDiscussionAnswerImage(int discussionAnswerId, int ImageId)
		{
			var discussionAnswer = _context.DiscussionAnswers.Where(d => d.Id == discussionAnswerId).FirstOrDefault();
			var image = _context.Images.Where(i => i.Id == ImageId).FirstOrDefault();
			var discussionImage = _context.DiscussionAnswerImages.Where(di => di.Image.Id == image.Id && di.DiscussionAnswer.Id == discussionAnswer.Id).FirstOrDefault();
			_context.Remove(discussionImage);
			_context.Remove(image);
			return Save();
		}

		public bool RemoveDiscussionAnswerLike(int id)
		{
			var discussionAnswer = _context.DiscussionAnswers.Where(d => d.Id == id).FirstOrDefault();
			discussionAnswer.Likes -= 1;
			return Save();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateDiscussionAnswer(DiscussionAnswer discussionAnswer)
		{
			_context.Update(discussionAnswer);
			return Save();
		}
	}
}
