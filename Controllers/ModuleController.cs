using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using somiod.DAL;
using somiod.Models;
using somiod.utils;

namespace somiod.Controllers{
	[ApiController]
	[Route("api/somiod/")] 
	public class ModuleController : CustomController{
		private readonly InheritanceMappingContext _context;
		
		public ModuleController(InheritanceMappingContext context){
			_context = context;
			res_type= Structures.res_type.module;
		}
		[HttpGet("{application}")]
		[Produces("application/xml")]
		public IActionResult Get([FromRoute]string application){
			var app = _context.Applications.SingleOrDefault(a => a.name == application);
			if (app == null){
				return UnprocessableEntity();
			}
			//Cast to DTO
			List<ModuleDTO> modules = new List<ModuleDTO>(_context.Modules.Where(m => m.parent == app).ToList().Select(m => new ModuleDTO(m)));
				//TODO oq fazer se a lista estiver vazia?
				return Ok(modules);
		}
		//Get module by id
		[HttpGet("{application}/{id:int}")]
		[Produces("application/xml")]
		public IActionResult GetSingle([FromRoute]string application, [FromRoute]int id){
			var mod = _context.Modules.Find(id);
			if (mod == null){
				return NotFound();
			}

			var app = _context.Applications.SingleOrDefault(a => a.name == application);
			if (app == null){
				return UnprocessableEntity();
			}
			return Ok(new ModuleDTO(mod));
		}
		//Create module
		[HttpPost("{application}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Post([FromRoute] string application, [FromBody]ModuleDTO moduleDTO){
			if(!preFlight(moduleDTO.res_type)){
				//Find a more apropriate code for this
				return UnprocessableEntity();
			}
			//check if application exists
			var app = _context.Applications.SingleOrDefault(a => a.name == application);
			if(app == null){
				return NotFound();
			}
			//check uniqueness
			if(_context.Modules.Any(a => a.name == moduleDTO.name)){
				return Conflict();
			}
			var mod=moduleDTO.fromDTO();
			mod.parent = app;

			_context.Modules.Add(mod);
			_context.SaveChanges();
			return Ok(moduleDTO);
		}
		//Update application
		[HttpPut("{application}/{name}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Put([FromRoute]string application, [FromRoute]string name,[FromBody]ModuleDTO moduleDTO){
			if(!preFlight(moduleDTO.res_type)){
				//Find a more apropriate code for this
				return UnprocessableEntity();
			}
			var mod = _context.Modules.SingleOrDefault(a => a.name == name);
			if(mod == null){
				return NotFound();
			}
			//check if application is the modules parent
			var app = _context.Applications.SingleOrDefault(a => a.name == application);
			if(app == null){
				Console.WriteLine("Application not found");
				return UnprocessableEntity();
			}
			if(mod.parent != app){
				Console.WriteLine("Application is not the modules parent");
				return UnprocessableEntity();
			}

			//check uniqueness
			if(_context.Modules.Any(a => a.name == moduleDTO.name)){
				return Conflict();
			}

			
			// NO id changes
			mod.name = moduleDTO.name;
			// TODO: SHOULD THIS BE CHANGED ON PUT?
			///app.creation_dt = application.creation_dt;

			_context.Modules.Update(mod);
			_context.SaveChanges();
			return Ok(new ModuleDTO(mod));
		}
		//Delete application
		[HttpDelete("{application}/{name}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Delete(string application, string name ){
			
			//check if application is the modules parent
			var app = _context.Applications.SingleOrDefault(a => a.name == application);
			if(app == null){
				Console.WriteLine("Application not found");
				return UnprocessableEntity();
				
			}
			var mod = _context.Modules.SingleOrDefault(a => a.name == name);
			if(mod == null){
				return NotFound();
			}
			
			if(mod.parent != app){
				Console.WriteLine("Application is not the modules parent");
				return NotFound();
			}
			
			_context.Modules.Remove(mod);
			_context.SaveChanges();
			return Ok(new ModuleDTO(mod));
		}


	}
	public class ModuleDTO{
		
        [Required]
		[MaxLength(20)]
		[RegularExpression(@"^[a-zA-Z\-_0-9]+", ErrorMessage = "Module names cant have any character thats not a latin letter, a numeral or the symbols hyphen and underscore")] 
		public string name { get; set; }
		[DefaultValue("module")]
		public string res_type { get; set; }

		public int? id { get; set; }

		public DateTime? creation_dt { get; set; }

		public ModuleDTO(string name){
			this.res_type = Structures.res_type_str[(int)Structures.res_type.module];
			this.name = name;
			this.creation_dt = DateTime.Now;
		} 

		public ModuleDTO(string content, int id):this(content){
			this.id = id;
		}
		public ModuleDTO(Module mod):this(mod.name, mod.id){
			this.creation_dt = mod.creation_dt;
		}
		public ModuleDTO():this("SampleModule"+DateTime.Now.ToString("yyyyMMddHHmmss")){}
		public Module fromDTO(){
			if(this.id == null){
				return new Module(this.name);
			}
			if(this.creation_dt == null){
				return new Module(this.name, this.id.Value, DateTime.Now);
			}
			return new Module(this.name, this.id.Value, this.creation_dt.Value);
		}
	}

	
	
}
