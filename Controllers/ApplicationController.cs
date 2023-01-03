using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using somiod.DAL;
using somiod.Models;
using somiod.utils;
using Swashbuckle.AspNetCore.Annotations;

namespace somiod.Controllers{
	[ApiController]
	[Route("api/somiod/")] 
	public class ApplicationController : CustomController{
		private readonly InheritanceMappingContext _context;
		
		public ApplicationController(InheritanceMappingContext context){
			_context = context;
			res_type= Structures.res_type.application;
		}

		[SwaggerOperation(Summary = "Gets all applications from the database", Description = "Returns a ArrayOfApplicationDTO object that wraps the actual dto objects" )]
		//[ProducesResponseType(typeof(ApplicationDTO[]), 200)]
		
		[HttpGet]
		[Produces("application/xml")]
		public IActionResult Get(){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{
				
				//Cast to DTO
				List<ApplicationDTO> applications = new List<ApplicationDTO>(_context.Applications.Select(a=> new ApplicationDTO(a)));
				return Ok(applications);
			}catch(Exception e){
				return BadRequest(e.Message);
			}
		}
		[SwaggerOperation(Summary = "Gets a single childless application from the database", Description = "Returns a ApplicationDTO object" )]
		[HttpGet("{id:int}")]
		[Produces("application/xml")]
		public IActionResult GetSingle(int id){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{
				var application = _context.Applications.Find(id);
				if (application == null){
					return NotFound("No such application found");
				}
				return Ok(new ApplicationDTO(application));
			}catch(Exception e){
				return BadRequest(e.Message);
			}
		}
		[SwaggerOperation(Summary = "Creates an application and posts it on the database", Description = "Returns the resulting ApplicationDTO" )]
		[HttpPost]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Post([FromBody]ApplicationDTO application){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{
				if(!preFlight(application.res_type)){
					return UnprocessableEntity("Invalid resource type");
				}
				var app=application.fromDTO();
				//check if application already exists
				if(_context.Applications.Any(a => a.name == app.name || a.id == application.id)){
					return Conflict("Application name already exists or the id is already in use");
				}
		

				_context.Applications.Add(app);
				_context.SaveChanges();
				return Ok(new ApplicationDTO(app));
			}catch(Exception e){
				return BadRequest(e.Message);
			}
		}
		[SwaggerOperation(Summary = "Updates an application name on the database", Description = "Returns the resulting ApplicationDTO, It wont update id nor creation_dt" )]
		[HttpPut("{name}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Put(string name,[FromBody]ApplicationDTO application){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{
				if(!preFlight(application.res_type)){
					//Find a more apropriate code for this
					return UnprocessableEntity("Invalid resource type");
				}
				var app = _context.Applications.SingleOrDefault(a => a.name == name);
				if(app == null){
					return NotFound("No such application found");
				}
				//check if application with name already exists
				if(_context.Applications.Any(a => a.name == application.name)){
					return Conflict("Application name already exists");
				}
				// NO id changes
				app.name = application.name;
				// SHOULD THIS BE CHANGED ON PUT?
				///app.creation_dt = application.creation_dt;

				_context.Applications.Update(app);
				_context.SaveChanges();
				return Ok(new ApplicationDTO(app));
			}catch(Exception e){
				return BadRequest(e.Message);
			}

		}
		[SwaggerOperation(Summary = "Deletes an application from the database", Description = "Returns the resulting ApplicationDTO, Will also remove any children associated(cascade)" )]
		[HttpDelete("{name}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Delete(string name ){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{
				var application = _context.Applications.SingleOrDefault(a => a.name == name);
				if(application == null){
					return NotFound("No such application found");
				}
				
				_context.Applications.Remove(application);
				_context.SaveChanges();
				return Ok(new ApplicationDTO(application));
			}catch(Exception e){
				return BadRequest(e.Message);
			}
		}


	}
	public class ApplicationDTO{
		[StringLength(20)]
		[DefaultValue("SampleApplication")]
		[RegularExpression(@"^[a-zA-Z\-_0-9]+", ErrorMessage = "Applications names cant have any character thats not a latin letter, a numeral or the symbols hyphen and underscore")] 
		public string name { get; set; }
		[DefaultValue("application")]
		public string res_type { get; set; }
		
		public int? id { get; set; }

		public DateTime? creation_dt { get; set; }

		public ApplicationDTO(string name){
			this.res_type = Structures.res_type_str[(int)Structures.res_type.application];
			this.name = name;
			this.creation_dt = DateTime.Now;
		} 

		public ApplicationDTO(string content, int id):this(content){
			this.id = id;
		}
		public ApplicationDTO(Application app):this(app.name, app.id){
			this.creation_dt = app.creation_dt;
		}
		public ApplicationDTO():this("SampleApplication"+DateTime.Now.ToString("yyyyMMddHHmmss")){}
		public Application fromDTO(){
			if(this.id == null){
				return new Application(this.name);
			}
			if(this.creation_dt == null){
				return new Application(this.name, this.id.Value, DateTime.Now);
			}
			return new Application(this.name, this.id.Value, this.creation_dt.Value);
		}
	}

	
	
}
