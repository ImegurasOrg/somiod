using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using somiod.DAL;
using somiod.Models;
using somiod.utils;

namespace somiod.Controllers{
    
    [ApiController]
	[Route("api/somiod/[controller]")]
	public class SubscriptionController: CustomController{
		private readonly InheritanceMappingContext _context;
		public SubscriptionController(InheritanceMappingContext context){
			_context = context;
            res_type = Structures.res_type.subscription;
		}
        //Through the REST API, it must be possible to create, modify, list, and delete each available resource. Data resources (records) and subscription resources only allow creation and deletion.
		[HttpPost("{application}/{module}")]
		[Consumes("application/xml")]
		[Produces("application/xml")]
		public IActionResult Post([FromRoute]string application, [FromRoute]string module, [FromBody]SubscriptionDTO subscriptionDTO){
			if(!preFlight(subscriptionDTO.res_type)){
				//Find a more apropriate code for this
				return UnprocessableEntity();
			}
			var mod = _context.Modules.SingleOrDefault(m => m.name == module);
			
			if(mod == null){
				return UnprocessableEntity();
			}
			//load parent
			_context.Entry(mod).Reference(m => m.parent).Load();
			if(mod.parent?.name != application){
				return UnprocessableEntity();
			}
			Subscription subscription = subscriptionDTO.fromDTO();
			subscription.parent=mod;
			_context.Add(subscription);
			_context.SaveChanges();
			return Ok(subscriptionDTO);
		}
		//HAS TO BE ID DUE TO NOT BEING UNIQUE
		[HttpDelete("{application}/{module}/{id}")]
		[Produces("application/xml")]
		public IActionResult Delete([FromRoute]string application, [FromRoute]string module, [FromRoute]int id){
			var sub = _context.Subscriptions.Find(id);
			if(sub == null){
				return NotFound();
			}


			var mod = _context.Modules.SingleOrDefault(m => m.name == module);
			if(mod == null){
				return UnprocessableEntity();
			}
			//load parent
			_context.Entry(mod).Reference(m => m.parent).Load();
			if(mod.parent?.name != application){
				return UnprocessableEntity();
			}
			
			_context.Remove(sub);
			_context.SaveChanges();
			return Ok(new SubscriptionDTO(sub));
		}
		
		
	}

	public class SubscriptionDTO{

		public int? id { get; set; }

		[DefaultValue("subscription")]
		public string res_type { get; set; }


	
		[DefaultValue("SampleSubscription")]
        public string name { get; set; }

		
		[DefaultValue("creation")]
		//value of @event is either "creation" or "deletion"
		[RegularExpression("creation|deletion")]
		public string @event {get;set;}
		//TODO: Maybe regex?
        public string endpoint {get; set;}

		private DateTime? creation_dt { get; set; }

        public SubscriptionDTO(string name, string @event, string endpoint){
			this.res_type = Structures.res_type_str[(int)Structures.res_type.subscription];
			this.name = name;
			this.@event = @event;
			this.endpoint = endpoint;
			this.creation_dt = DateTime.Now;


		} 
		public SubscriptionDTO(string name,string @event, string endpoint, int id):this(name, @event, endpoint){
			this.id = id;
		}
		public SubscriptionDTO(Subscription sub):this(sub.name,sub.@event, sub.endpoint, sub.id){
			this.creation_dt = sub.creation_dt;
		}
		public SubscriptionDTO():this("SampleSubscription"+DateTime.Now.ToString("yyyyMMddHHmmss"), "creation", "mqtt://13.38.228.158:1883"){}

		public Subscription fromDTO(){
			if(this.id == null){
				return new Subscription(this.name, this.@event, this.endpoint);
			}
			if(this.creation_dt == null){
				return new Subscription(this.name, this.@event, this.endpoint, this.id.Value, DateTime.Now);
			}
			return new Subscription(this.name, this.@event, this.endpoint, this.id.Value, this.creation_dt.Value);
		}
		//XML Serializer needs a default constructor
		
	}
}
