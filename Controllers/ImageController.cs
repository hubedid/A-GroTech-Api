using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace A_GroTech_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImageController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ResponseHelper _responseHelper;
		private readonly IImageRepository _imageRepository;

		public ImageController(IMapper mapper,
			ResponseHelper responseHelper,
			IImageRepository imageRepository)
		{
			_mapper = mapper;
			_responseHelper = responseHelper;
			_imageRepository = imageRepository;
		}

		[HttpGet("{imageId}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ImageDto>))]
		public IActionResult GetImages(int imageId)
		{
			try
			{
				var image = _mapper.Map<ImageDto>(_imageRepository.GetImage(imageId));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (image == null)
					return NotFound(_responseHelper.Error("No image found", 404));
				return Ok(_responseHelper.Success("", image));
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

		[HttpPut("{imageId}")]
		[ProducesResponseType(204)]
		public IActionResult UpdateImage(int imageId, [FromBody] ImagePostDto imagePostDto)
		{
			try
			{
				var image = _imageRepository.GetImage(imageId);
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (image == null)
					return NotFound(_responseHelper.Error("No image found", 404));

				_mapper.Map(imagePostDto, image);
				image.UpdatedAt = DateTime.Now;

				if (!_imageRepository.UpdateImage(image))
					throw new Exception("Failed to update image");

				return Ok(_responseHelper.Success("Image updated"));
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
