using A_GroTech_Api;
using A_GroTech_Api.Data;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using A_GroTech_Api.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<IDiscussionAnswerRepository, DiscussionAnswerRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ICommodityTypeRepository, CommodityTypeRepository>();
builder.Services.AddScoped<ICommodityRepository, CommodityRepository>();
builder.Services.AddScoped<IPredictionRepository, PredictionRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ResponseHelper>();
builder.Services.AddTransient<Seed>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "A-GroTech Hoaksss", Version = "v1" });

	// Konfigurasi untuk menambahkan header Bearer
	var securityScheme = new OpenApiSecurityScheme
	{
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		Description = "Enter 'token' for authorization"
	};
	c.AddSecurityDefinition("Bearer", securityScheme);

	var securityRequirement = new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	};
	c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddDbContext<DataContext>(options => {
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 6;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.User.RequireUniqueEmail = true;
	options.SignIn.RequireConfirmedEmail = true;
})
	.AddEntityFrameworkStores<DataContext>()
	.AddDefaultTokenProviders();
builder.Services.AddAuthentication(auth =>
{
	auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		};
		options.Events = new JwtBearerEvents
		{
			OnChallenge = async context =>
			{
				context.HandleResponse();
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				context.Response.ContentType = "application/json";
				var result = new UnauthorizedObjectResult(new ResponseHelper().Error("You are not authorized"));
				await result.ExecuteResultAsync(new ActionContext
				{
					HttpContext = context.HttpContext
				});
			}
		};
	});

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
	SeedData(app);

async void SeedData(IHost app)
{
	var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

	using (var scope = scopedFactory.CreateScope())
	{
		var service = scope.ServiceProvider.GetService<Seed>();
		await service.SeedDataContextAsync();
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
/*app.UseExceptionHandler(errorApp =>
{
	errorApp.Run(async context =>
	{
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		context.Response.ContentType = "application/json";

		var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
		var errorMessage = exceptionHandlerPathFeature?.Error.Message;

		// Cek apakah status code adalah 401 Unauthorized
		if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
		{
			var unauthorizedResponse = new
			{
				Message = "You are not authorized to access this resource."
			};

			var responseBody = JsonSerializer.Serialize(unauthorizedResponse);
			await context.Response.WriteAsync(responseBody);
		}
		else
		{
			var responseMessage = new
			{
				Message = "An error occurred while processing your request.",
				Error = errorMessage
			};

			var responseBody = JsonSerializer.Serialize(responseMessage);
			await context.Response.WriteAsync(responseBody);
		}
	});
});*/
/*app.Use(async (context, next) =>
{
	await next();
	if (context.Response.StatusCode == 404)
	{
		context.Response.ContentType = "application/json";
		var result = new NotFoundObjectResult(new ResponseHelper().Error("Not Found", 404));
		await result.ExecuteResultAsync(new ActionContext
		{
			HttpContext = context.Response.HttpContext
		});
	}
});*/
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
