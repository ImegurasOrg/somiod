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
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace somiod.Controllers{
    
    [ApiController]
	[Route("api/somiod/")]
	public class SubscriptionController: CustomController{
		private readonly InheritanceMappingContext _context;
		public SubscriptionController(InheritanceMappingContext context){
			_context = context;
            res_type = Structures.res_type.subscription;
		}
	
		[HttpPost("[controller]/{application}/{module}")]
		[Consumes("application/xml")]
		[Produces("application/xml")]
		public IActionResult Post([FromRoute]string application, [FromRoute]string module, [FromBody]SubscriptionDTO subscriptionDTO){
			if(!preFlight(subscriptionDTO.res_type)){
				//Find a more apropriate code for this
				return UnprocessableEntity("Invalid resource type");
			}
			//check if theres no conflicts with id
			var sub= _context.Subscriptions.SingleOrDefault(s => s.id == subscriptionDTO.id);
			if(sub != null){
				return Conflict("Subscription id already exists");
			}
			var mod = _context.Modules.SingleOrDefault(m => m.name == module);
			if(mod == null){
				return NotFound("No such module found, make shure the name is correct");
			}


			//load parent
			_context.Entry(mod).Reference(m => m.parent).Load();
			if(mod.parent?.name != application){
				return NotFound("The name of the modules parent doesnt match with application name, Did you mean: "+mod.parent?.name+"?");
			}
			Subscription subscription = subscriptionDTO.fromDTO();
			subscription.parent=mod;
			_context.Add(subscription);
			_context.SaveChanges();
			return Ok(subscriptionDTO);
		}
	
		[HttpDelete("{application}/{module}/{name}")]
		[Produces("application/xml")]
		public IActionResult Delete([FromRoute]string application, [FromRoute]string module, [FromRoute]string name){
			//s.parent != null is to disable warnings
			var subs = _context.Subscriptions.Include(s=>s.parent).DefaultIfEmpty().Where(s => s.name == name && s.parent!=null && s.parent.name == module).ToList();
			if(subs == null){
				return NotFound("No such subscription found, make shure the name is correct and that the module is its parent");
			}
			
			var l = subs.FirstOrDefault()?.parent;
			if(l!=null){
				_context.Entry(l).Reference(m => m.parent).Load();
				if(subs.FirstOrDefault()?.parent?.parent?.name != application){
					return NotFound("The name provided doesnt match with the name of the parent of the module. Did you mean: "+subs.FirstOrDefault()?.parent?.parent?.name +"?");
				}
				
				var k = new List<SubscriptionDTO>(subs.Select(a => new SubscriptionDTO(a)));
				//remove all subscriptions contained in subs
				_context.RemoveRange(subs);
				_context.SaveChanges();
				//return ok(subs) but subs has to be cast into SubscriptionDTO first
				
				return Ok(k);
			}
			return UnprocessableEntity("Apparently we found a subscription but the backend failed to retrieve a parent");
				
			
		}
	}

	public class SubscriptionDTO{

		
		[Required]
		[DefaultValue("SampleSubscription")]
		[MaxLength(50)]
        public string name { get; set; }

		[Required]
		[DefaultValue("creation")]
		[RegularExpression("creation|deletion")]
		public string @event {get;set;}
		//TODO: Maybe regex?
		//regular expression for mqtt
		//mqtt://[a-zA-Z0-9]+:[0-9]+
		
		[Required]
		[DefaultValue("mqtt://13.38.228.158:1883")]
		[RegularExpression(@"mqtt://[a-zA-Z0-9]+:[0-9]+|mqtt:\/\/(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]):[0-9]+$", ErrorMessage = "Invalid endpoint, valid endpoints are mqtt://domain:port or mqtt://ipv4:port")]
		public string endpoint {get; set;}
		
		[DefaultValue("subscription")]
		public string res_type { get; set; }

		public int? id { get; set; }

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
