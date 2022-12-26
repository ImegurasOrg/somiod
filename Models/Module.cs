using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace somiod.Models
{
    public class Module
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public DateTime creation_dt { get; set; }
		
		//[DefaultValue(null)]
		//public List<Data>? datas { get; set; }

       
        public Application? parent { get; set; } //Id of the module application
		public Module(string name){
			//datas=new List<Data>();
			this.name=name;
			//this.parent=parent;
			this.creation_dt=DateTime.Now;

		}
		//Blank constructor
		//Shouldnt be used
		public Module():this("DefaultModule"+DateTime.Now.ToString("yyyyMMddHHmmss")){}
    }
}
