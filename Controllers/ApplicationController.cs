using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using somiod.DAL;
using somiod.Models;
using somiod.utils;

namespace somiod.Controllers{
	[ApiController]
	[Route("api/somiod/")] 
	public class ApplicationController : ControllerBase{
		private readonly InheritanceMappingContext _context;
		public class applicationenvelope{
			//Forces the res_type to be application in swagger
			[DefaultValue("application")]
			public string res_type { get; set; }
			public Application application { get; set; }
			public applicationenvelope(string res_type, Application application){
				this.res_type = res_type;
				this.application = application;
			}

		}
		public ApplicationController(InheritanceMappingContext context){
			_context = context;
		}
		//true if we hit the right endpoint, else its not up to us
		[NonAction]
		public bool preFlight(string res_type){
			var util = Structures.instance; 
			if(res_type == util.parse_res_type(Structures.res_type.application)){
				return true;
			}
			return false; 
		}
		//Get all applications
		[HttpGet]
		[Produces("application/xml")]
		public IActionResult Get(){
			var applications = _context.Applications;
				//TODO oq fazer se a lista estiver vazia?
				return Ok(applications);
		}
		//Get application by id
		[HttpGet("{name}")]
		[Produces("application/xml")]
		public IActionResult GetSingle(string name){
			var application = _context.Applications.Single(a => a.name == name);
			if (application == null){
				return NotFound();
			}
			return Ok(application);
		}
		//Create application
		[HttpPost]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Post([FromBody]Application applicationEnvelope){
			/*if(!preFlight(applicationEnvelope.res_type)){
				//Find a more apropriate code for this
				return NotFound();
			}*/
			
			_context.Applications.Add(applicationEnvelope);
			_context.SaveChanges();
			return Ok(applicationEnvelope);
		}
		//Update application
		[HttpPut("{name}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Put(string name,[FromBody]applicationenvelope applicationEnvelope){
			if(!preFlight(applicationEnvelope.res_type)){
				//Find a more apropriate code for this
				return NotFound();
			}
			_context.Applications.Update(applicationEnvelope.application);
			_context.SaveChanges();
			return Ok(applicationEnvelope.application);
		}
		//Delete application
		[HttpDelete("{name}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Delete(string name ){
			
			/*
			[FromBody]string res_type
			if(!preFlight(res_type)){
				//Find a more apropriate code for this
				return NotFound();
			}*/
			var application = _context.Applications.Single(a => a.name == name);
			if(application == null)
				return NotFound();

			
			_context.Applications.Remove(application);
			_context.SaveChanges();
			return Ok(application);
		}


	}
}
