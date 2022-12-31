using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using somiod.utils;

namespace somiod.Models{
    public class Subscription{
		//data: id <int>; content <string>; creation_dt <string/ISO DateTime>; parent <int>
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
		
		[NotMapped]
		[DefaultValue("subscription")]
		public string res_type { get; set; }


        [Required]
		[DefaultValue("SampleSubscription")]
		[MaxLength(50)]
        public string name { get; set; }

		//event is reserved... the @ tells the compiler that its the actual variable name
		[Required]
		[DefaultValue("creation")]
		//value of @event is either "creation" or "deletion"
		[RegularExpression("creation|deletion")]
		public string @event {get;set;}
		//TODO: Maybe regex?
		[Required]
		[RegularExpression(@"mqtt://[a-zA-Z0-9]+:[0-9]+|mqtt:\/\/(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]):[0-9]+$", ErrorMessage = "Invalid endpoint, valid endpoints are mqtt://domain:port or mqtt://ipv4:port")]
        public string endpoint {get; set;}
		public DateTime? creation_dt { get; set; }
		
        public Module? parent { get; set; } //Id of the module application


		public Subscription(string name, string @event, string endpoint){

			this.name = name;
			this.@event = @event;
			this.endpoint = endpoint;
			this.res_type= Structures.res_type_str[(int)Structures.res_type.subscription];
		}
		//Blank constructor
		public Subscription():this("SampleSubscription"+DateTime.Now.ToString("yyyyMMddHHmmss"), "creation", "mqtt://13.38.228.158:1883"){}
		public Subscription(string name, string @event, string endpoint, int id, DateTime creation_dt):this(name, @event, endpoint){
			this.id=id;
			this.creation_dt=creation_dt;
		}
    }
	//This should be used as a d 
	
}
