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
	public class PredictionController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ResponseHelper _responseHelper;
		private readonly IPredictionRepository _predictionRepository;
		private readonly ICommodityRepository _commodityRepository;
		private readonly IAreaRepository _areaRepository;

		public PredictionController(IMapper mapper,
            ResponseHelper responseHelper,
            IPredictionRepository predictionRepository,
			ICommodityRepository commodityRepository,
			IAreaRepository areaRepository)
        {
			_mapper = mapper;
			_responseHelper = responseHelper;
			_predictionRepository = predictionRepository;
			_commodityRepository = commodityRepository;
			_areaRepository = areaRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<PredictionDto>))]
		public IActionResult GetPredictions()
		{
			try
			{
				var predictions = _mapper.Map<List<PredictionDto>>(_predictionRepository.GetPredictions());
			
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if(predictions.Any() != true)
					return NotFound(_responseHelper.Error("No predictions found", 404));
			
				return Ok(_responseHelper.Success("", predictions));
			}catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{predictionId}")]
		[ProducesResponseType(200, Type = typeof(PredictionDto))]
		public IActionResult GetPrediction(int predictionId)
		{
			try
			{
				var prediction = _mapper.Map<PredictionDto>(_predictionRepository.GetPrediction(predictionId));

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (prediction == null)
					return NotFound(_responseHelper.Error("No prediction found", 404));

				return Ok(_responseHelper.Success("", prediction));
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
		[Authorize(Roles = "Admin")]
		public IActionResult CreatePrediction([FromBody] PredictionPostDto predictionPostDto)
		{
			try
			{
				var prediction = _mapper.Map<Prediction>(predictionPostDto);
				var area = _areaRepository.GetArea(predictionPostDto.AreaId);
				if (area == null)
					return NotFound(_responseHelper.Error("Area not found", 404));
				var commodity = _commodityRepository.GetCommodity(predictionPostDto.CommodityId);
				if (commodity == null)
					return NotFound(_responseHelper.Error("Commodity not found", 404));
				prediction.Area = area;
				prediction.Commodity = commodity;
				prediction.CreatedAt = DateTime.Now;
				prediction.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if(!_predictionRepository.AddPrediction(prediction))
					throw new Exception("Failed to create prediction");

				return Ok(_responseHelper.Success("Prediction created successfully"));
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

		[HttpPut("{predictionId}")]
		[ProducesResponseType(204)]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdatePrediction(int predictionId, [FromBody] PredictionPostDto predictionPostDto)
		{
			try
			{
				var prediction = _predictionRepository.GetPrediction(predictionId);
				_mapper.Map(predictionPostDto, prediction);
				prediction.Area = _areaRepository.GetArea(predictionPostDto.AreaId);
				prediction.Commodity = _commodityRepository.GetCommodity(predictionPostDto.CommodityId);
				prediction.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_predictionRepository.UpdatePrediction(prediction))
					throw new Exception("Failed to update prediction");

				return Ok(_responseHelper.Success("Prediction updated successfully"));
			}catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpDelete("{predictionId}")]
		[ProducesResponseType(204)]
		[Authorize(Roles = "Admin")]
		public IActionResult DeletePrediction(int predictionId)
		{
			try
			{
				if (!_predictionRepository.DeletePrediction(predictionId))
					throw new Exception("Failed to delete prediction");

				return Ok(_responseHelper.Success("Prediction deleted successfully"));
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
