﻿using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IProductRepository
	{
		ICollection<Product> GetProducts(PaginationDto paginationDto);
		ICollection<Product> SearchProduct(string search, PaginationDto paginationDto);
		Product GetProduct(int id);
		bool AddProduct(Product product);
		bool UpdateProduct(Product product);
		bool DeleteProduct(int id);
		ICollection<Image> GetProductImages(int productId);
		ICollection<ProductReview> GetProductReviews(int productId, PaginationDto paginationDto);
		bool AddProductImage(int productId, List<ImagePostDto> image);
		bool RemoveProductImage(int productId, int ImageId);
		bool Save();
	}
}
