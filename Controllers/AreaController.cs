using A_GroTech_Api.Dto;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace A_GroTech_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AreaController : Controller
	{
		private readonly ResponseHelper _responseHelper;
		private readonly IAreaRepository _areaRepository;
		private readonly IMapper _mapper;

		public AreaController(ResponseHelper responseHelper, IAreaRepository areaRepository, IMapper mapper)
        {
			_responseHelper = responseHelper;
			_areaRepository = areaRepository;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<AreaDto>))]
		public IActionResult GetAreas()
		{
			try
			{
				var areas = _mapper.Map<List<AreaDto>>(_areaRepository.GetAreas());
				if(!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if(areas.Any() != true)
					return NotFound(_responseHelper.Error("No areas found", 404));
				return Ok(_responseHelper.Success("", areas));
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

		[HttpGet("{areaId}")]
		[ProducesResponseType(200, Type = typeof(AreaDto))]
		public IActionResult GetArea(int areaId)
		{
			try { 
				var area = _mapper.Map<AreaDto>(_areaRepository.GetArea(areaId));
				if(!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if(area == null)
					return NotFound(_responseHelper.Error("No area found", 404));
				return Ok(_responseHelper.Success("", area));
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
