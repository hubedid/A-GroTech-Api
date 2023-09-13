using A_GroTech_Api.Dto;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace A_GroTech_Api.Controllers
{
	[EnableCors("AllowAgilMilf")]
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : Controller
	{
		private readonly ResponseHelper _responseHelper;
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;

		public AuthController(ResponseHelper responseHelper, UserManager<User> userManager, IConfiguration configuration)
		{
			_responseHelper = responseHelper;
			_userManager = userManager;
			_configuration = configuration;
		}

        [HttpPost("Register")]
		public async Task<IActionResult> RegisterUser([FromBody] RegisterModel registerModel)
		{
			try
			{
				if (registerModel == null)
					return BadRequest(_responseHelper.Error("Invalid payload"));

				if (registerModel.Password != registerModel.ConfirmPassword)
					return BadRequest(_responseHelper.Error("Password and Confirm Password do not match"));

				var userAdd = new User
				{
					Name = registerModel.Name,
					UserName = registerModel.UserName,
					Email = registerModel.Email,
				};

				var result = await _userManager.CreateAsync(userAdd, registerModel.Password);
				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(userAdd, "User");
					return Ok(_responseHelper.Success("User created successfully"));
				}

				return BadRequest(_responseHelper.Error(result.Errors.FirstOrDefault()?.Description.ToString()));
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

		[HttpPost("Login")]
		[ProducesResponseType(204)]
		public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
		{
			try
			{
				if (loginModel == null)
					return BadRequest(_responseHelper.Error("Invalid payload"));

				var user = await _userManager.FindByNameAsync(loginModel.Email);
				if (user == null)
				{
					user = await _userManager.FindByEmailAsync(loginModel.Email);
					if(user == null)
						return BadRequest(_responseHelper.Error("User not found"));
				}

				var result = await _userManager.CheckPasswordAsync(user, loginModel.Password);
				if (!result)
					return BadRequest(_responseHelper.Error("Invalid password"));

				var roles = await _userManager.GetRolesAsync(user);
				var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
				var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
				var tokenExpires = DateTime.Now.AddDays(30);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Audience = _configuration["Jwt:Audience"],
					Issuer = _configuration["Jwt:Issuer"],
					Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(ClaimTypes.NameIdentifier, user.Id),
						new Claim(ClaimTypes.Name, user.Name),
						new Claim(ClaimTypes.Email, loginModel.Email),
						new Claim(ClaimTypes.Role, roles.FirstOrDefault())
					}),
					Expires = tokenExpires,
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
				};
				var securityToken = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
				var tokenString = jwtSecurityTokenHandler.WriteToken(securityToken);

				return Ok(_responseHelper.Success("", new { token = tokenString, expired = securityToken.ValidTo }));
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
