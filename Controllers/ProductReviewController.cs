using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using A_GroTech_Api.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace A_GroTech_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductReviewController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IProductReviewRepository _productReviewRepository;
		private readonly IUserRepository _userRepository;
		private readonly ResponseHelper _responseHelper;
		private readonly IProductRepository _productRepository;

		public ProductReviewController(IMapper mapper,
            ResponseHelper responseHelper,
            IProductRepository productRepository,
			IProductReviewRepository productReviewRepository,
			IUserRepository userRepository)
        {
			_mapper = mapper;
			_productReviewRepository = productReviewRepository;
			_userRepository = userRepository;
			_responseHelper = responseHelper;
			_productRepository = productRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ProductReviewDto>))]
		public IActionResult GetProductReviews()
		{
			try
			{
				var productReviews = _productReviewRepository.GetProductReviews();
				var productReviewsDto = _mapper.Map<List<ProductReviewDto>>(productReviews);
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				
				if (productReviews.Any() != true)
					return NotFound(_responseHelper.Error("No product review found", 404));
				return Ok(productReviewsDto);
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{productReviewId}")]
		[ProducesResponseType(200, Type = typeof(ProductReviewDto))]
		public IActionResult GetProductReview(int productReviewId)
		{
			try
			{
				var productReview = _productReviewRepository.GetProductReview(productReviewId);
				var productReviewDto = _mapper.Map<ProductReviewDto>(productReview);
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (productReview == null)
					return NotFound(_responseHelper.Error("No product review found", 404));
				return Ok(productReviewDto);
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpPost]
		[ProducesResponseType(204)]
		public IActionResult CreateProductReview([FromBody] ProductReviewPostDto productReviewPostDto)
		{
			try
			{
				var productReview = _mapper.Map<ProductReview>(productReviewPostDto);
				var user = _userRepository.GetUser(productReviewPostDto.ReviewedById);
				var product = _productRepository.GetProduct(productReviewPostDto.ProductId);
				productReview.ReviewedBy = user;
				productReview.Product = product;
				productReview.CreatedAt = DateTime.Now;
				productReview.UpdatedAt = DateTime.Now;
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (user == null)
					return NotFound(_responseHelper.Error("No user found", 404));
				if (product == null)
					return NotFound(_responseHelper.Error("No product found", 404));

				_productReviewRepository.AddProductReview(productReview);
				return Ok(_responseHelper.Success("Product review created successfully"));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpPut("{productReviewId}")]
		[ProducesResponseType(204)]
		public IActionResult UpdateProductReview(int productReviewId, [FromBody] ProductReviewPutDto productReviewPutDto)
		{
			try
			{
				var productReview = _productReviewRepository.GetProductReview(productReviewId);
				_mapper.Map(productReviewPutDto, productReview);
				productReview.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				
				if(!_productReviewRepository.UpdateProductReview(productReview))
					throw new Exception("Failed to update product review");

				return Ok(_responseHelper.Success("Product review updated successfully"));
			}
			catch(SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpDelete("{productReviewId}")]
		[ProducesResponseType(204)]
		public IActionResult DeleteProductReview(int productReviewId)
		{
			try
			{
				if (!_productReviewRepository.DeleteProductReview(productReviewId))
					throw new Exception("Failed to delete product review");

				return Ok(_responseHelper.Success("Product review deleted successfully"));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{productReviewId}/images")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ImageDto>))]
		public IActionResult GetProductReviewImages(int productReviewId)
		{
			try
			{
				var images = _mapper.Map<List<ImageDto>>(_productReviewRepository.GetProductReviewImages(productReviewId));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (images.Any() != true)
					return NotFound(_responseHelper.Error("No images found", 404));
				return Ok(_responseHelper.Success("", images));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpPost("{productReviewId}/images")]
		[ProducesResponseType(204)]
		public IActionResult AddProductReviewImage(int productReviewId, [FromBody] List<ImagePostDto> imagePostDto)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_productReviewRepository.AddProductReviewImage(productReviewId, imagePostDto))
				{
					throw new Exception("Creating an image failed on save.");
				}
				return Ok(_responseHelper.Success("Image created successfully"));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpDelete("{productReviewId}/images")]
		[ProducesResponseType(204)]
		public IActionResult RemoveProductImage(int productReviewId, [Required][FromQuery] int imageId)
		{
			try
			{
				if (!_productReviewRepository.RemoveProductReviewImage(productReviewId, imageId))
					throw new Exception("Something went wrong in deleting ProductReview image");

				return Ok(_responseHelper.Success("Product image deleted successfuly"));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpPatch("{productReviewId}/like")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult AddLikesToProductReview(int productReviewId)
		{
			try
			{
				if (!_productReviewRepository.AddLike(productReviewId))
				{
					throw new Exception("Adding likes to ProductReview failed on save.");
				}
				return Ok(_responseHelper.Success("Likes added to ProductReview successfully"));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpDelete("{productReviewId}/like")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult RemoveLikesFromProductReview(int productReviewId)
		{
			try
			{
				if (!_productReviewRepository.RemoveLike(productReviewId))
				{
					throw new Exception("Removing likes from ProductReview failed on save.");
				}
				return Ok(_responseHelper.Success("Likes removed from ProductReview successfully"));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}
	}
}
