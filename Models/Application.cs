using System.ComponentModel.DataAnnotations;

namespace somiod.Models{
	public class Application{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public DateTime Creation_DT { get; set; }
	}
}