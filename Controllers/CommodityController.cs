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
	public class CommodityController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ResponseHelper _responseHelper;
		private readonly ICommodityRepository _commodityRepository;
		private readonly ICommodityTypeRepository _commodityTypeRepository;

		public CommodityController(IMapper mapper,
            ResponseHelper responseHelper,
            ICommodityRepository commodityRepository,
			ICommodityTypeRepository commodityTypeRepository)
        {
			_mapper = mapper;
			_responseHelper = responseHelper;
			_commodityRepository = commodityRepository;
			_commodityTypeRepository = commodityTypeRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<CommodityDto>))]
		public IActionResult GetCommodities([FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var commodities = _mapper.Map<List<CommodityDto>>(_commodityRepository.GetCommodities(paginationDto));

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (commodities.Any() != true)
					return NotFound(_responseHelper.Error("No commodities found", 404));

				return Ok(_responseHelper.Success("", commodities));
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

		[HttpGet("{commodityId}/areas")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<AreaDto>))]
		public IActionResult GetCommodityAreas(int commodityId, [FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var commodityAreas = _mapper.Map<List<AreaDto>>(_commodityRepository.GetCommodityAreas(commodityId, paginationDto));

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (commodityAreas.Any() != true)
					return NotFound(_responseHelper.Error("No commodity areas found", 404));

				return Ok(_responseHelper.Success("", commodityAreas));
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

		[HttpPost("{commodityId}/areas")]
		[ProducesResponseType(204)]
		public IActionResult CreateCommodityArea(int commodityId, [FromBody] AreaPostDto commodityAreaBodyModel)
		{
			try
			{
				var commodityArea = _mapper.Map<Area>(commodityAreaBodyModel);
				commodityArea.CreatedAt = DateTime.Now;
				commodityArea.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				_commodityRepository.AddCommodityArea(commodityId, commodityArea);

				return Ok(_responseHelper.Success("Commodity area created"));
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

		[HttpPatch("{commodityId}/areas")]
		[ProducesResponseType(204)]
		public IActionResult AddCommodityAreaById(int commodityId, [FromQuery] int areaId)
		{
			try
			{
				_commodityRepository.AddCommodityAreaById(commodityId, areaId);

				return Ok(_responseHelper.Success("Commodity area created"));
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

		[HttpGet("{commodityId}")]
		[ProducesResponseType(200, Type = typeof(CommodityDto))]
		public IActionResult GetCommodity(int commodityId)
		{
			try
			{
				var commodity = _mapper.Map<CommodityDto>(_commodityRepository.GetCommodity(commodityId));

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (commodity == null)
					return NotFound(_responseHelper.Error("No commodity found", 404));

				return Ok(_responseHelper.Success("", commodity));
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
		public IActionResult CreateCommodity([FromBody] CommodityPostDto commodityPostDto)
		{
			try
			{
				var commodity = _mapper.Map<Commodity>(commodityPostDto);
				commodity.CommodityType = _commodityTypeRepository.GetCommodityType(commodityPostDto.CommodityTypeId);
				commodity.CreatedAt = DateTime.Now;
				commodity.UpdatedAt = DateTime.Now;

				if(!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if(!_commodityRepository.AddCommodity(commodity))
					throw new Exception("Something went wrong in sql execution");

				return Ok(_responseHelper.Success("Commodity created successfully"));
			}catch(SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch(Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpPut("{commodityId}")]
		[ProducesResponseType(204)]
		public IActionResult UpdateCommodity(int commodityId, [FromBody] CommodityPutDto commodityPutDto)
		{
			try
			{
				var commodity = _commodityRepository.GetCommodity(commodityId);
				if (commodity == null)
					return NotFound(_responseHelper.Error("No commodity found", 404));
				
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				
				commodity = _mapper.Map(commodityPutDto, commodity);
				commodity.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_commodityRepository.UpdateCommodity(commodity))
					throw new Exception("Something went wrong in sql execution");

				return Ok(_responseHelper.Success("Commodity updated successfully"));
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

		[HttpDelete("{commodityId}")]
		[ProducesResponseType(204)]
		public IActionResult DeleteCommodity(int commodityId)
		{
			try
			{
				if (!_commodityRepository.DeleteCommodity(commodityId))
					throw new Exception("Something went wrong in sql execution");

				return Ok(_responseHelper.Success("Commodity deleted successfully"));
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
