using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using somiod.utils;

namespace somiod.Models{
    public class Data{
		//data: id <int>; content <string>; creation_dt <string/ISO DateTime>; parent <int>
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
		
		[NotMapped]
		[DefaultValue("data")]
		public string res_type { get; set; }


        [Required]
		[DefaultValue("SampleData")]
		[MaxLength(50)]
		public string content { get; set; }

        
        public DateTime? creation_dt { get; set; }
		
        public Module? parent { get; set; } //Id of the module application


		public Data(string content){
			this.content = content;
			//this.parent = parent;
			this.res_type= Structures.res_type_str[(int)Structures.res_type.data];
		}
		//Blank constructor
		public Data():this("SampleData"+DateTime.Now.ToString("yyyyMMddHHmmss")){}
		public Data(string content, int id, DateTime creation_dt):this(content){
			this.id=id;
			this.creation_dt=creation_dt;
		}
    }
	//This should be used as a d 
	
}
