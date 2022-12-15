using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace somiod.Models{
	public class Application{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		[Required]
		public string name { get; set; }
		[Required]
		public DateTime creation_dt { get; set; }
		
	}
}