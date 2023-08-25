using A_GroTech_Api.Data;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Repository
{
	public class AreaRepository : IAreaRepository
	{
		private readonly DataContext _dataContext;

		public AreaRepository(DataContext dataContext)
        {
			_dataContext = dataContext;
		}
        public Area GetArea(int id)
		{
			return _dataContext.Areas.Where(a => a.Id == id).FirstOrDefault();
		}

		public ICollection<Area> GetAreas()
		{
			return _dataContext.Areas.ToList();
		}
	}
}
