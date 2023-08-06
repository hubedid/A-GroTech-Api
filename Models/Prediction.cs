namespace A_GroTech_Api.Models
{
	public class Prediction
	{
        public int Id { get; set; }
        public long Price { get; set; }
        public DateTime Date { get; set; }
        public Commodity Commodity { get; set; }
        public Area Area { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
