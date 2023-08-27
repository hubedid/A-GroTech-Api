using A_GroTech_Api.Data;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public ProductRepository(DataContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}
        public bool AddProduct(Product product)
		{
			_context.Add(product);
			return Save();
		}

		public bool AddProductImage(int productId, List<ImagePostDto> image)
		{
			var product = _context.Products.Where(d => d.Id == productId).FirstOrDefault();
			foreach (var item in image)
			{
				var imageMap = _mapper.Map<Image>(item);
				imageMap.CreatedAt = DateTime.Now;
				imageMap.UpdatedAt = DateTime.Now;
				var productImage = new ProductImage
				{
					Product = product,
					Image = imageMap
				};
				_context.Add(productImage);
			}
			return Save();
		}

		public bool DeleteProduct(int id)
		{
			var product = _context.Products.Find(id);
			_context.Remove(product);
			return Save();
		}

		public Product GetProduct(int id)
		{
			var product = _context.Products
				.Where(p => p.Id == id)
				.Include(p => p.Commodity)
				.Include(p => p.Area)
				.FirstOrDefault();
			return _mapper.Map<Product>(product);
		}

		public ICollection<Image> GetProductImages(int productId)
		{
			var productImages = _context.ProductImages
				.Where(p => p.Product.Id == productId)
				.Select(p => p.Image)
				.ToList();
			return _mapper.Map<ICollection<Image>>(productImages);
		}

		public ICollection<ProductReview> GetProductReviews(int productId)
		{
			var productReviews = _context.ProductReviews
				.Where(p => p.Product.Id == productId)
				.Include(p => p.ReviewedBy)
				.ToList();
			return _mapper.Map<ICollection<ProductReview>>(productReviews);
		}

		public ICollection<Product> GetProducts()
		{
			var products = _context.Products
				.Include(p => p.Commodity)
				.Include(p => p.Area)
				.ToList();
			return _mapper.Map<ICollection<Product>>(products);
		}

		public bool RemoveProductImage(int productId, int ImageId)
		{
			var image = _context.Images.Find(ImageId);
			var productImage = _context.ProductImages
				.Where(p => p.Product.Id == productId && p.Image.Id == ImageId)
				.FirstOrDefault();
			_context.Remove(productImage);
			_context.Remove(image);
			return Save();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public ICollection<Product> SearchProduct(string search)
		{
			var products = _context.Products
				.Where(p => p.Name.Contains(search) || p.Description.Contains(search) || p.Commodity.Name.Contains(search))
				.Include(p => p.Commodity)
				.Include(p => p.Area)
				.ToList();
			return _mapper.Map<ICollection<Product>>(products);
		}

		public bool UpdateProduct(Product product)
		{
			_context.Update(product);
			return Save();
		}
	}
}
