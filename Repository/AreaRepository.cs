using A_GroTech_Api.Data;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Repository
{
	public class AreaRepository : IAreaRepository
	{
		private readonly DataContext _context;

		public AreaRepository(DataContext context)
        {
			_context = context;
		}

		public bool AddArea(Area area)
		{
			_context.Add(area);
			return Save();
		}

		public bool DeleteArea(int id)
		{
			var area = _context.Areas.Find(id);
			_context.Remove(area);
			return Save();
		}

		public Area GetArea(int id)
		{
			return _context.Areas.Where(a => a.Id == id).FirstOrDefault();
		}

		public ICollection<Area> GetAreas()
		{
			return _context.Areas.ToList();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateArea(Area area)
		{
			_context.Update(area);
			return Save();
		}
	}
}
