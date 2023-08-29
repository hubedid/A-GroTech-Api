using A_GroTech_Api.Data;
using A_GroTech_Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api
{
	public class Seed
	{
		private readonly DataContext _context;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public Seed(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		/* * * * * *  Login Data * * * * * *
		 * Admin						   *
		 * Email	: admin@admin.com	   *
		 * Password	: antihoaks123		   *
		 *								   *
		 * User							   *
		 * Email	: usertes1@example.com *
		 * Password	: walangkecek123	   *
		 * * * * * * * * * * * * * * * * * */
		public async Task SeedDataContextAsync()
		{
			if (!await _roleManager.RoleExistsAsync("Admin"))
			{
				await _roleManager.CreateAsync(new IdentityRole("Admin"));
				await _roleManager.CreateAsync(new IdentityRole("User"));

				var admin = new User
				{
					Name = "Admin",
					UserName = "admin",
					Email = "admin@admin.com"
				};
				var addAdmin = await _userManager.CreateAsync(admin, "antihoaks123");
				if (addAdmin.Succeeded)
				{
					await _userManager.AddToRoleAsync(admin, "Admin");
					Console.WriteLine($"User {admin.UserName} created successfully.");
				}
				else
				{
					Console.WriteLine($"User {admin.UserName} failed to create.");
				}

				var userNew = new List<User>()
				{
					new User
					{
						Name = "yahaha",
						UserName = "tes123",
						Email = "usertes1@example.com"
					},
					new User
					{
						Name = "hayukkk",
						UserName = "tes234",
						Email = "usertes2@example.com"
					},
				};
				foreach(var users in userNew)
				{
					try
					{
						await _userManager.CreateAsync(users, "walangkecek123");
						await _userManager.AddToRoleAsync(users, "User");
						Console.WriteLine($"User {users.UserName} created successfully.");
					} 
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
				}

				var imageNew = new List<Image>(){
					new Image
					{
						Path = "https://fastly.picsum.photos/id/12/2500/1667.jpg?hmac=Pe3284luVre9ZqNzv1jMFpLihFI6lwq7TPgMSsNXw2w",
						CreatedAt = DateTime.Now,
						UpdatedAt = DateTime.Now,
					},
					new Image
					{
						Path = "https://fastly.picsum.photos/id/9/5000/3269.jpg?hmac=cZKbaLeduq7rNB8X-bigYO8bvPIWtT-mh8GRXtU3vPc",
						CreatedAt = DateTime.Now,
						UpdatedAt = DateTime.Now,
					},
					new Image
					{
						Path = "https://fastly.picsum.photos/id/7/4728/3168.jpg?hmac=c5B5tfYFM9blHHMhuu4UKmhnbZoJqrzNOP9xjkV4w3o",
						CreatedAt = DateTime.Now,
						UpdatedAt = DateTime.Now,
					},
					new Image
					{
						Path = "https://fastly.picsum.photos/id/28/4928/3264.jpg?hmac=GnYF-RnBUg44PFfU5pcw_Qs0ReOyStdnZ8MtQWJqTfA",
						CreatedAt = DateTime.Now,
						UpdatedAt = DateTime.Now,
					}
				};
				var imageRangeAdd = _context.Images.AddRangeAsync(imageNew);
				if (imageRangeAdd.IsCompletedSuccessfully)
				{
					var imageAdd = await _context.SaveChangesAsync();
					if (imageAdd > 0)
						Console.WriteLine("Image seeding successfully");
				}
				var areaNew = new List<Area>()
				{
					new Area
					{
						Provinsi = "Jawa Barat",
						Kota = "Bandung",
						Kecamatan = "Cimahi Selatan",
						Latitude = "-6.872",
						Longitude = "107.542",
						CreatedAt = DateTime.Now,
						UpdatedAt = DateTime.Now,
					},
					new Area
					{
						Provinsi = "Jawa Barat",
						Kota = "Bandung",
						Kecamatan = "Cimahi Utara",
						Latitude = "-6.872",
						Longitude = "107.542",
						CreatedAt = DateTime.Now,
						UpdatedAt = DateTime.Now,
					},
					new Area
					{
						Provinsi = "Jawa Barat",
						Kota = "Bandung",
						Kecamatan = "Cimahi Tengah",
						Latitude = "-6.872",
						Longitude = "107.542",
						CreatedAt = DateTime.Now,
						UpdatedAt = DateTime.Now,
					}
				};
				var areaRangeAdd = _context.Areas.AddRangeAsync(areaNew);
				if (areaRangeAdd.IsCompletedSuccessfully)
				{
					var areaAdd = await _context.SaveChangesAsync();
					if (areaAdd > 0)
						Console.WriteLine("Area seeding successfully");
				};
				var commodityTypeNew = new CommodityType
				{
					Name = "Sayur",
					Description = "Sayur atau sayuran merupakan sebutan umum bagi bahan pangan nabati yang biasanya mengandung kadar air yang tinggi, yang dapat dikonsumsi setelah dimasak atau diolah dengan teknik tertentu, atau dalam keadaan segar. Istilah untuk kumpulan berbagai jenis sayur adalah sayur-sayuran atau sayur-mayur.",
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
				};
				_context.CommodityTypes.Add(commodityTypeNew);
				var commodityTypeSave = await _context.SaveChangesAsync();
				if (commodityTypeSave > 0)
					Console.WriteLine("Commodity Type seeding successfully");

				var user1 = await _userManager.FindByEmailAsync("usertes1@example.com");
				var user2 = await _userManager.FindByEmailAsync("usertes2@example.com");
				var image1 = await _context.Images.Where(i => i.Path == "https://fastly.picsum.photos/id/12/2500/1667.jpg?hmac=Pe3284luVre9ZqNzv1jMFpLihFI6lwq7TPgMSsNXw2w").FirstOrDefaultAsync();
				var image2 = await _context.Images.Where(i => i.Path == "https://fastly.picsum.photos/id/9/5000/3269.jpg?hmac=cZKbaLeduq7rNB8X-bigYO8bvPIWtT-mh8GRXtU3vPc").FirstOrDefaultAsync();
				var image3 = await _context.Images.Where(i => i.Path == "https://fastly.picsum.photos/id/7/4728/3168.jpg?hmac=c5B5tfYFM9blHHMhuu4UKmhnbZoJqrzNOP9xjkV4w3o").FirstOrDefaultAsync();
				var image4 = await _context.Images.Where(i => i.Path == "https://fastly.picsum.photos/id/28/4928/3264.jpg?hmac=GnYF-RnBUg44PFfU5pcw_Qs0ReOyStdnZ8MtQWJqTfA").FirstOrDefaultAsync();
				var area1 = await _context.Areas.Where(a => a.Kecamatan == "Cimahi Selatan").FirstOrDefaultAsync();
				var area2 = await _context.Areas.Where(a => a.Kecamatan == "Cimahi Utara").FirstOrDefaultAsync();
				var area3 = await _context.Areas.Where(a => a.Kecamatan == "Cimahi Tengah").FirstOrDefaultAsync();
				var commodityType1 = await _context.CommodityTypes.Where(c => c.Name == "Sayur").FirstOrDefaultAsync();

				var userArea = new List<UserArea>(){
					new UserArea
					{
						User = user1,
						Area = area1,
					},
					new UserArea
					{
						User = user1,
						Area = area2,
					},

				};
				var userAreaAdd = _context.UserAreas.AddRangeAsync(userArea);
				if (userAreaAdd.IsCompletedSuccessfully)
				{
					var userAreaSave = await _context.SaveChangesAsync();
					if (userAreaSave > 0)
						Console.WriteLine("User Area seeding successfully");
				}
				var discussion = new Discussion
				{
					Tittle = "Sistem Irigasi yang Efisien",
					Message = "Rina adalah seorang petani yang memiliki lahan pertanian di daerah yang cenderung kering. Setelah musim tanam terakhir yang kurang sukses akibat kurangnya air, ia ingin mencari solusi irigasi yang efisien untuk pertanaman berikutnya. Bagaimana Anda akan memberi saran pada Rina mengenai pilihan sistem irigasi yang sesuai?",
					Likes = 1,
					User = user1,
					IsSolved = false,
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
				};
				var discussionImage = new DiscussionImage
				{
					Image = image1,
					Discussion = discussion,
				};
				_context.DiscussionImages.Add(discussionImage);
				var discussionImageSave = await _context.SaveChangesAsync();
					if (discussionImageSave > 0)
						Console.WriteLine("Discussion Image seeding successfully");

				var discussionAnswer = new DiscussionAnswer
				{
					Message = "Tentu, Rina. Karena lahan Anda cenderung kering, penting untuk memilih sistem irigasi yang dapat mengirimkan air secara efisien ke tanaman. Sistem irigasi tetes atau sistem irigasi tetesan adalah pilihan yang baik. Sistem ini menyuplai air langsung ke akar tanaman melalui pipa tetesan kecil, mengurangi kebuangan air karena penguapan atau aliran berlebih. Ini membantu Anda menghemat air dan mengalirkan ke air hanya ke area yang dibutuhkan. Selain itu, mempertimbangkan penggunaan alat monitoring tanah atau sensor kelembaban dapat membantu Anda mengetahui kapan tanaman perlu air tambahan, sehingga Anda dapat mengoptimalkan irigasi.",
					Likes = 1,
					AnsweredBy = user2,
					Discussion = _context.Discussions.Where(d => d.User == user1).FirstOrDefault(),
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
				};
				_context.DiscussionAnswers.Add(discussionAnswer);
				var discussionAnswerSave = await _context.SaveChangesAsync();
					if (discussionAnswerSave > 0)
						Console.WriteLine("Discussion Answer seeding successfully");
				
				var pinnedDiscussionAnswer = new PinnedDiscussionAnswer
				{
					Discussion = _context.Discussions.Where(d => d.User == user1).FirstOrDefault(),
					DiscussionAnswer = _context.DiscussionAnswers.Where(d => d.AnsweredBy == user2).FirstOrDefault(),
				};
				_context.PinnedDiscussionAnswers.Add(pinnedDiscussionAnswer);
				var pinnedDiscussionAnswerSave = await _context.SaveChangesAsync();
				if (pinnedDiscussionAnswerSave > 0)
					Console.WriteLine("Pinned Discussion Answer seeding successfully");
				
				var discussionAnswerImage = new DiscussionAnswerImage
				{
					DiscussionAnswer = _context.DiscussionAnswers.Where(d => d.AnsweredBy == user2).FirstOrDefault(),
					Image = image2
				};
				_context.DiscussionAnswerImages.Add(discussionAnswerImage);
				var discussionAnswerImageSave = await _context.SaveChangesAsync();
				if (discussionAnswerImageSave > 0)
					Console.WriteLine("Discussion Answer Image seeding successfully");
				

				var notification = new Notification
				{
					Message = "Hoaks telah menambahkan diskusi baru",
					IsRead = false,
					User = user1,
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
				};
				_context.Notifications.Add(notification);
				var notificationSave = await _context.SaveChangesAsync();
				if (notificationSave > 0)
					Console.WriteLine("Notification seeding successfully");
				
				_context.NotificationImages.Add(new NotificationImage
				{
					Notification = _context.Notifications.Where(n => n.User == user1).FirstOrDefault(),
					Image = image3
				});
				var notificationImageSave = await _context.SaveChangesAsync();
				if (notificationImageSave > 0)
					Console.WriteLine("Notification Image seeding successfully");
				
				var commodity = new Commodity
				{
					Name = "Sayur Daun",
					Description = "Sayuran daun, juga disebut sayuran daun hijau, sayuran hijau, atau sekadar sayuran hijau, adalah daun tanaman yang dimakan sebagai sayuran, terkadang disertai dengan tangkai daun dan pucuk yang empuk. Sayuran daun yang dimakan mentah dalam salad bisa disebut salad greens.", 
					CommodityType = commodityType1,
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
				};
				_context.Commodities.Add(commodity);
				var commoditySave = await _context.SaveChangesAsync();
				if (commoditySave > 0)
					Console.WriteLine("Commodity seeding successfully");
				
				var commodityArea = new CommodityArea
				{
					Commodity = _context.Commodities.Where(c => c.Name == "Sayur Daun").FirstOrDefault(),
					Area = area2,
				};
				_context.CommodityAreas.Add(commodityArea);
				var commodityAreaSave = await _context.SaveChangesAsync();
				if (commodityAreaSave > 0)
					Console.WriteLine("Commodity Area seeding successfully");
				
				var product = new Product
				{
					Name = "Kangkung",
					Description = "Kangkung adalah tumbuhan yang termasuk jenis sayur-sayuran dan ditanam sebagai makanan. Kangkung banyak dijual di pasar-pasar. Kangkung banyak terdapat di kawasan Asia, tempat asalnya tidak diketahui. dan merupakan tumbuhan yang dapat dijumpai hampir di mana-mana terutama di kawasan berair.",
					PriceUnit = "Kg",
					Price = 10000,
					Stock = 100,
					Commodity = _context.Commodities.Where(c => c.Name == "Sayur Daun").FirstOrDefault(),
					Area = area2,
					User = user1,
					Status = 1,
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
				};
				_context.Products.Add(product);
				var productSave = await _context.SaveChangesAsync();
				if (productSave > 0)
					Console.WriteLine("Product seeding successfully");

				var productImage = new ProductImage
				{
					Product = _context.Products.Where(p => p.Name == "Kangkung").FirstOrDefault(),
					Image = image4
				};
				_context.ProductImages.Add(productImage);
				var productImageSave = await _context.SaveChangesAsync();
				if (productImageSave > 0)
					Console.WriteLine("Product Image seeding successfully");
				
				var order = new Order
				{
					Product = _context.Products.Where(p => p.Name == "Kangkung").FirstOrDefault(),
					Buyer = user2,
					Quantity = 10,
					Notes = "Yang bagus-bagus mas",
					Status = "Dikemas",
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
				};
				_context.Orders.Add(order);
				var orderSave = await _context.SaveChangesAsync();
				if (orderSave > 0)
					Console.WriteLine("Order seeding successfully");

				var productReview = new ProductReview
				{
					Product = _context.Products.Where(p => p.Name == "Kangkung").FirstOrDefault(),
					ReviewedBy = user2,
					Message = "Bagus mas sayurnya fresh mantappp",
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
				};
				
				var predictionNew = new List<Prediction>()
				{
					new Prediction
					{
						Price = 1000000,
						Date = DateTime.Now.AddDays(20),
						Commodity = _context.Commodities.Where(c => c.Name == "Sayur Daun").FirstOrDefault(),
						Area = area3,
						CreatedAt = DateTime.Now,
						UpdatedAt = DateTime.Now,
					},
					new Prediction
					{
						Price = 2000000,
						Date = DateTime.Now.AddDays(20),
						Commodity = _context.Commodities.Where(c => c.Name == "Sayur Daun").FirstOrDefault(),
						Area = area2,
						CreatedAt = DateTime.Now,
						UpdatedAt = DateTime.Now,
					},
					new Prediction
					{
						Price = 3000000,
						Date = DateTime.Now.AddDays(20),
						Commodity = _context.Commodities.Where(c => c.Name == "Sayur Daun").FirstOrDefault(),
						Area = area1,
						CreatedAt = DateTime.Now,
						UpdatedAt = DateTime.Now,
					},
				};
				var predictionAdd = _context.Predictions.AddRangeAsync(predictionNew);
				if (predictionAdd.IsCompletedSuccessfully)
				{
					var predictionSave = await _context.SaveChangesAsync();
					if (predictionSave > 0)
						Console.WriteLine("Prediction seeding successfully");
				}
				Console.WriteLine("Seeding successfully");
			}
			else
			{
				Console.WriteLine("Already seeded");
			}
		}
    }
}
