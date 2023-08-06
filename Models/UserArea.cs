namespace A_GroTech_Api.Models
{
	public class UserArea
	{
        public int UserId { get; set; }
		public int AreaId { get; set; }
		public User User { get; set; }
		public Area Area { get; set; }
	}
}
