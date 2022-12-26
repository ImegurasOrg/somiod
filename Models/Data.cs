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
        public string content { get; set; }

        
        public DateTime? creation_dt { get; set; }
		
        [Required]
        public Module parent { get; set; } //Id of the module application
		public Data(string content){
			this.content = content;
			//this.parent = parent;
			this.res_type= Structures.res_type_str[(int)Structures.res_type.data];
		}
		//Blank constructor
		public Data():this("SampleData"){}

    }
	//This should be used as a d 
	
}
