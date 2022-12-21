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

        [Required]
        public Application parent { get; set; } //Id of the module application
		public Module(string name, Application parent){
			this.name=name;
			this.parent=parent;
			this.creation_dt=DateTime.Now;

		}
		//Blank constructor
		//Shouldnt be used
		public Module():this("DefaultModule"+DateTime.Now.ToString("yyyyMMddHHmmss"), new Application()){}
    }
}
