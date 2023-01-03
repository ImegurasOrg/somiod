using System.Collections;
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
	public class ModuleController : CustomController{
		private readonly InheritanceMappingContext _context;
		
		public ModuleController(InheritanceMappingContext context){
			_context = context;
			res_type= Structures.res_type.module;
		}
		[SwaggerOperation(Summary = "Gets all child modules from a certain application", Description = "Returns a ArrayOfModuleDTO object that wraps the actual dto objects" )]
		[HttpGet("{application}")]
		[Produces("application/xml")]
		public IActionResult Get([FromRoute]string application){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{
				var app = _context.Applications.SingleOrDefault(a => a.name == application);
				if (app == null){
					return NotFound("Application not found");
				}
				//Cast to DTO
				List<ModuleDTO> modules = new List<ModuleDTO>(_context.Modules.Where(m => m.parent == app).ToList().Select(m => new ModuleDTO(m)));
					//TODO oq fazer se a lista estiver vazia?
					return Ok(modules);
			}catch(Exception e){
				return BadRequest(e.Message);
			}

		}
		[SwaggerOperation(Summary = "Gets a single (childless) module from a certain application", Description = "Returns a ModuleDTO object" )]
		//Get module by id
		[HttpGet("{application}/{id:int}")]
		[Produces("application/xml")]
		public IActionResult GetSingle([FromRoute]string application, [FromRoute]int id){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{
				var mod = _context.Modules.Find(id);
				if (mod == null){
					return NotFound("No such module found");
				}

				_context.Entry(mod).Reference(m => m.parent).Load();
				if(mod.parent?.name != application){	
					return NotFound("Modules father is not the application provided. Did You mean to use"+mod.parent?.name);
				}
				return Ok(new ModuleDTO(mod));
			}catch(Exception e){
				return BadRequest(e.Message);
			}
		}
		[SwaggerOperation(Summary = "Gets a single module with his data children from a certain application", Description = "Returns a ModuleWithDataDTO object" )]
		//get module 
		[HttpGet("{application}/[controller]/{name}")]
		[Produces("application/xml")]
		public IActionResult GetSinglet([FromRoute]string application, [FromRoute]string name){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{

				var mod = _context.Modules.SingleOrDefault(m => m.name == name);
				if (mod == null){
					return NotFound("No such module found");
				}

				_context.Entry(mod).Reference(m => m.parent).Load();
				if(mod.parent?.name != application){	
					return NotFound("Modules father is not the application provided. Did You mean to use"+mod.parent?.name);
				}
				//fill 
				var k =new ModuleWithDataDTO(mod);
				//Load collection
				_context.Entry(mod).Collection(m => m.datas).Load();
				k.dataList = _context.Data.Where(d => d.parent == mod).Select(d => new DataDTO(d)).ToList();
				return Ok(k);
			}catch(Exception e){
				return BadRequest(e.Message);
			}

		}
		[SwaggerOperation(Summary = "Creates a new module", Description = "Returns the provided ModuleDTO object" )]
		//Create module
		[HttpPost("{application}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Post([FromRoute] string application, [FromBody]ModuleDTO moduleDTO){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{

				if(!preFlight(moduleDTO.res_type)){
					//Find a more apropriate code for this
					return UnprocessableEntity("Invalid resource type");
				}
				//check if application exists
				var app = _context.Applications.SingleOrDefault(a => a.name == application);
				if(app == null){
					return NotFound("No such application found");
				}
				//check uniqueness
				if(_context.Modules.Any(a => a.name == moduleDTO.name|| a.id == moduleDTO.id)){
					return Conflict("Either a module with this name already exists or the id is already in use");
				}
				var mod=moduleDTO.fromDTO();
				mod.parent = app;

				_context.Modules.Add(mod);
				_context.SaveChanges();
				return Ok(new ModuleDTO(mod));
			}catch(Exception e){
				return BadRequest(e.Message);
			}

		}
		[SwaggerOperation(Summary = "Updates a module name", Description = "Returns the provided ModuleDTO object" )]
		//Update application
		[HttpPut("{application}/{name}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Put([FromRoute]string application, [FromRoute]string name,[FromBody]ModuleDTO moduleDTO){
			//this is not optimal but since the teachers don't really want us to care for prolongued usage and will penalise us for any untreated errors its a must
			try{
				if(!preFlight(moduleDTO.res_type)){
					//Find a more apropriate code for this
					return UnprocessableEntity("Invalid resource type");
				}
				var mod = _context.Modules.SingleOrDefault(a => a.name == name);
				if(mod == null){
					return NotFound("No module with such name exists");
				}
				//check if application is the modules parent;
				_context.Entry(mod).Reference(m => m.parent).Load();

				if(mod.parent?.name != application){
					return NotFound("The application provided isnt the modules parent. Did you meant to say: "+mod.parent?.name+"?");
				}

				//check uniqueness
				if(_context.Modules.Any(a => a.name == moduleDTO.name)){
					return Conflict("A module with this name already exists");
				}

				
				// NO id changes
				mod.name = moduleDTO.name;
				// TODO: SHOULD THIS BE CHANGED ON PUT?
				///app.creation_dt = application.creation_dt;

				_context.Modules.Update(mod);
				_context.SaveChanges();
				return Ok(new ModuleDTO(mod));
			}catch(Exception e){
				return BadRequest(e.Message);
			}
		}
		[SwaggerOperation(Summary = "Deletes a module", Description = "Returns the provided ModuleDTO object, will also remove any children associated with this module(cascade)" )]
		//Delete application
		[HttpDelete("{application}/{name}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Delete(string application, string name ){
			try{
				var mod = _context.Modules.SingleOrDefault(a => a.name == name);
				if(mod == null){
					return NotFound("No such module found");
				}
				_context.Entry(mod).Reference(m => m.parent).Load();
				if(mod.parent?.name != application){
					return NotFound("The application provided isnt the modules parent. Did you meant to say: "+mod.parent?.name+"?");
				}
				
				_context.Modules.Remove(mod);
				_context.SaveChanges();
				return Ok(new ModuleDTO(mod));
			}catch(Exception e){
				return BadRequest(e.Message);
			}

		}


	}
	public class ModuleDTO{
		
        [Required]
		[MaxLength(20)]
		[RegularExpression(@"^[a-zA-Z\-_0-9]+", ErrorMessage = "Module names cant have any character thats not a latin letter, a numeral or the symbols hyphen and underscore")] 
		[DefaultValue("SampleModule")]
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

		public ModuleDTO(string name, int id):this(name){
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
	public class ModuleWithDataDTO:ModuleDTO{
		
		public List<DataDTO> dataList { get; set; }
		
		public ModuleWithDataDTO(string name):base(name){
			this.dataList = new List<DataDTO>();
		}
		public ModuleWithDataDTO(string name, int id):base(name, id){
			this.dataList = new List<DataDTO>();
		}
		public ModuleWithDataDTO(Module mod):base(mod){
			this.dataList = new List<DataDTO>();
		}
		public ModuleWithDataDTO():base(){
			this.dataList = new List<DataDTO>();
		}

	}

	
	
}
