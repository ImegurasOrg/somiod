using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace somiod.Models{
	public class Application{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		[Required]
		[DefaultValue("DefaultApplication")] // this is just for documentation purposes it does ""nothing""
		public string name { get; set; }
		
		public DateTime? creation_dt { get; set; }
		
		public Application(string name){
			this.name = name;
			creation_dt = DateTime.Now;
		}
		//Blank constructor 
		public Application(): this("DefaultApplication"+DateTime.Now.ToString("yyyyMMddHHmmss")){}
		
	}
}