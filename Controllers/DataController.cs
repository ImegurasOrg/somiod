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
using Swashbuckle.AspNetCore.Annotations;

namespace somiod.Controllers{
    
    [ApiController]
	[Route("api/somiod/")]
	public class DataController: CustomController{
		private readonly InheritanceMappingContext _context;
		public DataController(InheritanceMappingContext context){
			_context = context;
            res_type = Structures.res_type.data;
		}
        //Through the REST API, it must be possible to create, modify, list, and delete each available resource. Data resources (records) and subscription resources only allow creation and deletion.
		[SwaggerOperation(Summary = "Creates a new data resource", Description = "Returns the DataDTO provided, when launched will also message to the module channel subscriber")]
		[HttpPost("{application}/{module}")]
		[Consumes("application/xml")]
		[Produces("application/xml")]
		public IActionResult Post([FromRoute]string application, [FromRoute]string module, [FromBody]DataDTO dataDTO){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{
				if(!preFlight(dataDTO.res_type)){
					//Find a more apropriate code for this
					return UnprocessableEntity("Invalid resource type");
				}
				//check if theres no conflicts with id
				var data_= _context.Data.SingleOrDefault(d => d.id == dataDTO.id);
				if(data_ != null){
					return Conflict("Data id is already in use");
				}

				var mod = _context.Modules.SingleOrDefault(m => m.name == module);
				
				if(mod == null){
					return NotFound("Module not found");
				}
				//load parent
				_context.Entry(mod).Reference(m => m.parent).Load();
				if(mod.parent?.name != application){	
					return NotFound("Modules father is not the application provided. Did You mean to use"+mod.parent?.name);
				}
				Data data = dataDTO.fromDTO();
				data.parent=mod;
				_context.Add(data);
				_context.SaveChanges();
				//broker vem das subscriptions em que tenho o module como parent e event created but not deleted? , mod = topic e data = message
				//query get subscriptions where module = mod
				//subctract from count created - count deleted
				//if count > 0 then send message(topic = mod, message = data)
				List<OmeuTipo>? result = Helper.CheckSubscritions(module, _context);
				if(result != null){
					foreach(var item in result){
						//todo: if count < 0 something is wrong
						if(item.Count > 0){
							//todo: send xml in message

							//this is intentional the server neither awaits nor cares about the result of the async call
							#pragma warning disable CS4014 
							Helper.PublishAsync(item.Endpoint, module, dataDTO);
							#pragma warning restore CS4014
						}
					}
				} else {
					Console.WriteLine("Cannot send notification");	
				}
				return Ok(new DataDTO(data));
			} catch(Exception e){
				return BadRequest(e.Message);
			}

		}
		[SwaggerOperation(Summary = "Deletes a certain data resource", Description = "Returns the deleted DataDTO")]
		[HttpDelete("{application}/{module}/{id:int}")]
		[Produces("application/xml")]
		public IActionResult Delete([FromRoute]string application, [FromRoute]string module, [FromRoute]int id){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{
				var data = _context.Data.Find(id);
				if(data == null){
					return NotFound("No such Data not found");
				}


				var mod = _context.Modules.SingleOrDefault(m => m.name == module);
				if(mod == null){
					return NotFound("Module not found");
				}
				//load parent
				_context.Entry(mod).Reference(m => m.parent).Load();
				if(mod.parent?.name != application){
					return NotFound("Modules father is not the application provided. Did You mean to use"+mod.parent?.name);
				}
				
				_context.Remove(data);
				_context.SaveChanges();
				return Ok(new DataDTO(data));
			} catch(Exception e){
				return BadRequest(e.Message);
			}
		}
		
		
	}

	public class DataDTO{

		
		[Required]
		[DefaultValue("SampleData")]
		[MaxLength(50)]
		public string content { get; set; }
		
		[DefaultValue("data")]
		public string res_type { get; set; }
		
		public int? id { get; set; }
		

		private DateTime? creation_dt { get; set; }

        public DataDTO(string content){
			this.res_type = Structures.res_type_str[(int)Structures.res_type.data];
			this.content = content;
			this.creation_dt = DateTime.Now;


		} 
		public DataDTO(string content, int id):this(content){
			this.id = id;
		}
		public DataDTO(Data data):this(data.content, data.id){
			this.creation_dt = data.creation_dt;
		}
		public DataDTO():this("SampleData"+DateTime.Now.ToString("yyyyMMddHHmmss")){}

		public Data fromDTO(){
			if(this.id == null){
				return new Data(this.content);
			}
			if(this.creation_dt == null){
				return new Data(this.content, this.id.Value, DateTime.Now);
			}
			return new Data(this.content, this.id.Value, this.creation_dt.Value);
		}
		//XML Serializer needs a default constructor
		
	}
}
