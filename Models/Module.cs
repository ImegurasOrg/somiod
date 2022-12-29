﻿using System.ComponentModel;
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
		
		
		public virtual ICollection<Data> datas { get; set; }

       
        public Application? parent { get; set; } //Id of the module application
		public Module(string name){
			
			this.name=name;
			//this.parent=parent;
			this.creation_dt=DateTime.Now;
			datas=new List<Data>();

		}
		public Module(string name, int id, DateTime creation_dt):this(name){
			this.id=id;
			this.creation_dt=creation_dt;
			
		}
		//Blank constructor
		//Shouldnt be used
		public Module():this("DefaultModule"+DateTime.Now.ToString("yyyyMMddHHmmss")){}
    }
}
