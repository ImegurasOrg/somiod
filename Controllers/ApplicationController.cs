using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using somiod.DAL;
using somiod.Models;
using somiod.utils;

namespace somiod.Controllers{
	[ApiController]
	[Route("api/somiod/")] 
	public class ApplicationController : CustomController{
		private readonly InheritanceMappingContext _context;
		
		public ApplicationController(InheritanceMappingContext context){
			_context = context;
			res_type= Structures.res_type.application;
		}
	
		//Get all applications
		[HttpGet]
		[Produces("application/xml")]
		public IActionResult Get(){
			//Cast to DTO
			List<ApplicationDTO> applications = new List<ApplicationDTO>(_context.Applications.ToList().Select(a => new ApplicationDTO(a)));
				//TODO oq fazer se a lista estiver vazia?
				return Ok(applications);
		}
		//Get application by id
		[HttpGet("{id}")]
		[Produces("application/xml")]
		public IActionResult GetSingle(int id){

			var application = _context.Applications.Find(id);
			if (application == null){
				return NotFound();
			}
			return Ok(new ApplicationDTO(application));
		}
		//Create application
		[HttpPost]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Post([FromBody]ApplicationDTO application){
			if(!preFlight(application.res_type)){
				//Find a more apropriate code for this
				return UnprocessableEntity();
			}
			var app=application.fromDTO();
			_context.Applications.Add(app);
			_context.SaveChanges();
			return Ok(app);
		}
		//Update application
		[HttpPut("{name}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Put(string name,[FromBody]ApplicationDTO application){
			if(!preFlight(application.res_type)){
				//Find a more apropriate code for this
				return UnprocessableEntity();
			}
			var app = _context.Applications.SingleOrDefault(a => a.name == name);
			if(app == null){
				return NotFound();
			}
			// NO id changes
			app.name = application.name;
			// TODO: SHOULD THIS BE CHANGED ON PUT?
			///app.creation_dt = application.creation_dt;

			_context.Applications.Update(app);
			_context.SaveChanges();
			return Ok(new ApplicationDTO(app));
		}
		//Delete application
		[HttpDelete("{name}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Delete(string name ){
			
			var application = _context.Applications.SingleOrDefault(a => a.name == name);
			if(application == null)
				return NotFound();

			
			_context.Applications.Remove(application);
			_context.SaveChanges();
			return Ok(new ApplicationDTO(application));
		}


	}
	public class ApplicationDTO{
		public int? id { get; set; }
		[DefaultValue("SampleApplication")]
		public string name { get; set; }
		[DefaultValue("application")]
		public string res_type { get; set; }

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
