using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A_GroTech_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin, User")]
	public class CommodityTypeController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ResponseHelper _responseHelper;
		private readonly ICommodityTypeRepository _commodityTypeRepository;

		public CommodityTypeController(IMapper mapper, 
            ResponseHelper responseHelper, 
            ICommodityTypeRepository commodityTypeRepository)
        {
			_mapper = mapper;
			_responseHelper = responseHelper;
			_commodityTypeRepository = commodityTypeRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<CommodityTypeDto>))]
		public IActionResult GetCommodityTypes([FromQuery] PaginationDto paginationDto)
		{
			try { 				
				var commodityTypes = _mapper.Map<List<CommodityTypeDto>>(_commodityTypeRepository.GetCommodityTypes(paginationDto));
			
				if (!ModelState.IsValid)
							return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if(commodityTypes.Any() != true)
							return NotFound(_responseHelper.Error("No commodity types found", 404));
			
				return Ok(_responseHelper.Success("", commodityTypes));
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

		[HttpGet("{commodityTypeId}")]
		[ProducesResponseType(200, Type = typeof(CommodityTypeDto))]
		public IActionResult GetCommodityType(int commodityTypeId)
		{
			try
			{
				var commodityType = _mapper.Map<CommodityTypeDto>(_commodityTypeRepository.GetCommodityType(commodityTypeId));

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (commodityType == null)
					return NotFound(_responseHelper.Error("No commodity type found", 404));

				return Ok(_responseHelper.Success("", commodityType));
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
		[ProducesResponseType(200, Type = typeof(CommodityTypeDto))]
		public IActionResult CreateCommodityType([FromBody] CommodityTypePostDto commodityTypePostDto)
		{
			try
			{
				var commodityType = _mapper.Map<CommodityType>(commodityTypePostDto);
				commodityType.CreatedAt = DateTime.Now;
				commodityType.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (!_commodityTypeRepository.AddCommodityType(commodityType))
					throw new Exception("Commodity type not created");

				return Ok(_responseHelper.Success("Commodity type created successfuly", commodityType));
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

		[HttpPut("{commodityTypeId}")]
		[ProducesResponseType(200, Type = typeof(CommodityTypeDto))]
		public IActionResult UpdateCommodityType(int commodityTypeId, [FromBody] CommodityTypePostDto commodityTypePostDto)
		{
			try
			{
				var commodityType = _commodityTypeRepository.GetCommodityType(commodityTypeId);
				if (commodityType == null)
					return NotFound(_responseHelper.Error("No commodity type found", 404));
				commodityType = _mapper.Map(commodityTypePostDto, commodityType);
				commodityType.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (!_commodityTypeRepository.UpdateCommodityType(commodityType))
					throw new Exception("Commodity type not updated");

				return Ok(_responseHelper.Success("Commodity type updated successfuly", commodityType));
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

		[HttpDelete("{commodityTypeId}")]
		[ProducesResponseType(200, Type = typeof(CommodityTypeDto))]
		public IActionResult DeleteCommodityType(int commodityTypeId)
		{
			try
			{
				
				if (!_commodityTypeRepository.DeleteCommodityType(commodityTypeId))
					throw new Exception("Commodity type not deleted");
				return Ok(_responseHelper.Success("Commodity type deleted successfuly"));
			}catch (SqlException ex)
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
