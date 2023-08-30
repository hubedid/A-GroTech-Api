using A_GroTech_Api.Data;
using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Repository
{
	public class ProductReviewRepository : IProductReviewRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public ProductReviewRepository(DataContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		public bool AddLike(int id)
		{
			var productReview = _context.ProductReviews.Find(id);
			productReview.Likes += 1;
			return Save();
		}

		public bool AddProductReview(ProductReview productReview)
		{
			_context.Add(productReview);
			return Save();
		}

		public bool AddProductReviewImage(int productReviewId, List<ImagePostDto> image)
		{
			var productReview = _context.ProductReviews.Where(d => d.Id == productReviewId).FirstOrDefault();
			foreach (var item in image)
			{
				var imageMap = _mapper.Map<Image>(item);
				imageMap.CreatedAt = DateTime.Now;
				imageMap.UpdatedAt = DateTime.Now;
				var productReviewImage = new ProductReviewImage
				{
					ProductReview = productReview,
					Image = imageMap
				};
				_context.Add(productReviewImage);
			}
			return Save();
		}

		public bool DeleteProductReview(int id)
		{
			var productReview = _context.ProductReviews.Find(id);
			_context.Remove(productReview);
			return Save();
		}

		public ProductReview GetProductReview(int id)
		{
			var productReview = _context.ProductReviews
				.Where(d => d.Id == id)
				.Include(d => d.ReviewedBy)
				.Include(d => d.Product)
				.FirstOrDefault();
			return _mapper.Map<ProductReview>(productReview);
		}

		public ICollection<Image> GetProductReviewImages(int productReviewId)
		{
			var productReviewImages = _context.ProductReviewImages
				.Where(di => di.ProductReview.Id == productReviewId)
				.Include(d => d.Image)
				.Select(d => d.Image)
				.ToList();
			return _mapper.Map<ICollection<Image>>(productReviewImages);
		}

		public ICollection<ProductReview> GetProductReviews(PaginationDto paginationDto)
		{
			var productReviews = _context.ProductReviews
				.Include(d => d.ReviewedBy)
				.Include(d => d.Product)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<ProductReview>>(productReviews);
		}

		public bool RemoveLike(int id)
		{
			var productReview = _context.ProductReviews.Find(id);
			productReview.Likes -= 1;
			return Save();
		}

		public bool RemoveProductReviewImage(int productReviewId, int imageId)
		{
			var image = _context.Images.Where(i => i.Id == imageId).FirstOrDefault();
			var productReviewImage = _context.ProductReviewImages
				.Where(di => di.ProductReview.Id == productReviewId && di.Image.Id == imageId)
				.FirstOrDefault();
			_context.Remove(productReviewImage);
			_context.Remove(image);
			return Save();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved >= 0 ? true : false;
		}

		public bool UpdateProductReview(ProductReview productReview)
		{
			_context.Update(productReview);
			return Save();
		}
	}
}
