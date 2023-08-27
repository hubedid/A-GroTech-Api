using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Dto;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace A_GroTech_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ResponseHelper _responseHelper;
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly IUserRepository _userRepository;

		public OrderController(IMapper mapper,
            ResponseHelper responseHelper,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUserRepository userRepository)
        {
			_mapper = mapper;
			_responseHelper = responseHelper;
			_orderRepository = orderRepository;
			_productRepository = productRepository;
			_userRepository = userRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<OrderDto>))]
		public IActionResult GetOrders()
		{
			try
			{
				var orders = _orderRepository.GetOrders();
				var ordersDto = _mapper.Map<List<OrderDto>>(orders);
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (orders.Any() != true)
					return NotFound(_responseHelper.Error("No order found", 404));
				return Ok(ordersDto);
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

		[HttpGet("{orderId}")]
		[ProducesResponseType(200, Type = typeof(OrderDto))]
		public IActionResult Getorder(int orderId)
		{
			try
			{
				var order = _orderRepository.GetOrder(orderId);
				var orderDto = _mapper.Map<OrderDto>(order);
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (order == null)
					return NotFound(_responseHelper.Error("No order found", 404));
				return Ok(orderDto);
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
		public IActionResult Createorder([FromBody] OrderPostDto orderPostDto)
		{
			try
			{
				var order = _mapper.Map<Order>(orderPostDto);
				var user = _userRepository.GetUser(orderPostDto.BuyerId);
				var product = _productRepository.GetProduct(orderPostDto.ProductId);
				order.Buyer = user;
				order.Product = product;
				order.CreatedAt = DateTime.Now;
				order.UpdatedAt = DateTime.Now;
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (user == null)
					return NotFound(_responseHelper.Error("No user found", 404));
				if (product == null)
					return NotFound(_responseHelper.Error("No product found", 404));

				_orderRepository.AddOrder(order);
				return Ok(_responseHelper.Success("order created successfully"));
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

		[HttpPut("{orderId}")]
		[ProducesResponseType(204)]
		public IActionResult Updateorder(int orderId, [FromBody] OrderPutDto orderPutDto)
		{
			try
			{
				var order = _orderRepository.GetOrder(orderId);
				_mapper.Map(orderPutDto, order);
				order.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_orderRepository.UpdateOrder(order))
					throw new Exception("Failed to update order");

				return Ok(_responseHelper.Success("order updated successfully"));
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

		[HttpDelete("{orderId}")]
		[ProducesResponseType(204)]
		public IActionResult Deleteorder(int orderId)
		{
			try
			{
				if (!_orderRepository.DeleteOrder(orderId))
					throw new Exception("Failed to delete order");

				return Ok(_responseHelper.Success("order deleted successfully"));
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
