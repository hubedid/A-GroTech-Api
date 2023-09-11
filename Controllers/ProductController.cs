using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace A_GroTech_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin, User")]
	public class ProductController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ResponseHelper _responseHelper;
		private readonly IProductRepository _productRepository;
		private readonly ICommodityRepository _commodityRepository;
		private readonly IAreaRepository _areaRepository;
		private readonly IUserRepository _userRepository;

		public ProductController(IMapper mapper,
            ResponseHelper responseHelper,
            IProductRepository productRepository,
            ICommodityRepository commodityRepository,
            IAreaRepository areaRepository,
            IUserRepository userRepository)
        {
			_mapper = mapper;
			_responseHelper = responseHelper;
			_productRepository = productRepository;
			_commodityRepository = commodityRepository;
			_areaRepository = areaRepository;
			_userRepository = userRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
		public IActionResult GetProducts([FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var products = _mapper.Map<List<ProductDto>>(_productRepository.GetProducts(paginationDto));

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (products.Any() != true)
					return Ok(_responseHelper.Success("No products found"));

				return Ok(_responseHelper.Success("", products));
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
		[ProducesResponseType(400)]
		public IActionResult AddProduct([FromBody] ProductPostDto productPostDto)
		{
			try
			{
				var product = _mapper.Map<Product>(productPostDto);
				product.Commodity = _commodityRepository.GetCommodity(productPostDto.CommodityId);
				product.Area = _areaRepository.GetArea(productPostDto.AreaId);
				product.User = _userRepository.GetUser(productPostDto.OwnerId);
				product.CreatedAt = DateTime.Now;
				product.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if(product.Commodity == null)
					return NotFound(_responseHelper.Error("Commodity not found", 404));
				if (product.Area == null)
					return NotFound(_responseHelper.Error("Area not found", 404));
				if (product.User == null)
					return NotFound(_responseHelper.Error("User not found", 404));
				
				if(!_productRepository.AddProduct(product))
					throw new Exception("Something went wrong while adding product");

				return Ok(_responseHelper.Success("Product added successfully"));
			}
			catch(SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch(Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{productId}")]
		[ProducesResponseType(200, Type = typeof(ProductDto))]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult GetProduct(int productId)
		{
			try
			{
				var product = _mapper.Map<ProductDto>(_productRepository.GetProduct(productId));

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (product == null)
					return NotFound(_responseHelper.Error("Product not found", 404));

				return Ok(_responseHelper.Success("", product));
			}
			catch(SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch(Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpPut("{productId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult UpdateProduct(int productId, [FromBody] ProductPutDto productPutDto)
		{
			try
			{
				var product = _productRepository.GetProduct(productId);
				_mapper.Map(productPutDto, product);
				product.Commodity = _commodityRepository.GetCommodity(productPutDto.CommodityId);
				product.Area = _areaRepository.GetArea(productPutDto.AreaId);
				product.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (product == null)
					return NotFound(_responseHelper.Error("Product not found", 404));
				if (product.Commodity == null)
					return NotFound(_responseHelper.Error("Commodity not found", 404));
				if (product.Area == null)
					return NotFound(_responseHelper.Error("Area not found", 404));

				if (!_productRepository.UpdateProduct(product))
					throw new Exception("Something went wrong while updating product");

				return Ok(_responseHelper.Success("Product updated successfully"));
			}
			catch(SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch(Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpDelete("{productId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public IActionResult DeleteProduct(int productId)
		{
			try
			{
				_productRepository.DeleteProduct(productId);
				return Ok(_responseHelper.Success("Product deleted successfully"));
			}catch(SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch(Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{productId}/reviews")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ProductReviewDto>))]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult GetProductReviews(int productId, [FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var productReviews = _productRepository.GetProductReviews(productId, paginationDto);
				var productReviewsMap = _mapper.Map<List<ProductReviewDto>>(productReviews);

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (productReviewsMap.Any() != true)
					return NotFound(_responseHelper.Error("Product reviews not found", 404));

				return Ok(_responseHelper.Success("", productReviewsMap));
			}
			catch(SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch(Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}


		[HttpGet("search")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult SearchProducts([FromQuery] string search, [FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var result = _productRepository.SearchProduct(search, paginationDto);
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				
				if (result.Any() != true)
					return NotFound(_responseHelper.Error("No products found", 404));

				var resultMap = _mapper.Map<List<ProductDto>>(result);
				return Ok(_responseHelper.Success("", resultMap));
			}
			catch(SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch(Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{productId}/images")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ImageDto>))]
		public IActionResult GetDiscussionAnswerImages(int productId)
		{
			try
			{
				var images = _mapper.Map<List<ImageDto>>(_productRepository.GetProductImages(productId));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (images == null)
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

		[HttpPost("{productId}/images")]
		[ProducesResponseType(204)]
		public IActionResult AddDiscussionAnswerImage(int productId, [FromBody] List<ImagePostDto> imagePostDto)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_productRepository.AddProductImage(productId, imagePostDto))
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

		[HttpDelete("{productId}/images")]
		[ProducesResponseType(204)]
		public IActionResult DeleteDiscussionAnswerImage(int productId, [Required][FromQuery] int imageId)
		{
			try
			{
				if (!_productRepository.RemoveProductImage(productId, imageId))
					throw new Exception("Something went wrong in deleting Product image");

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
	}
}
