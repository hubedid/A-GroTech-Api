using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IProductReviewRepository
	{
		ICollection<ProductReview> GetProductReviews();
		ProductReview GetProductReview(int id);
		bool AddProductReview(ProductReview productReview);
		bool UpdateProductReview(ProductReview productReview);
		bool DeleteProductReview(int id);
		ICollection<Image> GetProductReviewImages(int productReviewId);
		bool AddProductReviewImage(int productReviewId, List<ImagePostDto> image);
		bool RemoveProductReviewImage(int productReviewId, int imageId);
		bool Save();
		bool AddLike(int id);
		bool RemoveLike(int id);
	}
}
