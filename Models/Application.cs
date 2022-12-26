using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using somiod.utils;

namespace somiod.Models{
	public class Application{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		[Required]
		[StringLength(20)]
		[DefaultValue("DefaultApplication")] // this is just for documentation purposes it does ""nothing""
		public string name { get; set; }
		
		public DateTime? creation_dt { get; set; }
		
		
		//Res_type is not to go on the database
		[NotMapped]
		[DefaultValue("application")]
		public string res_type { get; set; }

		public virtual ICollection<Module> modules { get; set; }

		public Application(string name){
			//this.modules = new List<Module>();
			this.res_type=Structures.parse_res_type(Structures.res_type.application);
			this.name = name;
			creation_dt = DateTime.Now;
			this.modules=new List<Module>();
		}
		//Blank constructor 
		public Application(): this("DefaultApplication"+DateTime.Now.ToString("yyyyMMddHHmmss")){}
		
	}
}