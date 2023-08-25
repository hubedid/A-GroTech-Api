using A_GroTech_Api.Data;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Repository
{
	public class ImageRepository : IImageRepository
	{
		private readonly DataContext _context;

		public ImageRepository(DataContext context)
        {
			_context = context;
		}
        public Image GetImage(int id)
		{
			return _context.Images.Find(id);
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateImage(Image image)
		{
			_context.Update(image);
			return Save();
			throw new NotImplementedException();
		}
	}
}
