using System.ComponentModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using somiod.DAL;
using somiod.Models;
using somiod.utils;

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
		 [HttpPost("{application}/{module}")]
		 [Produces("application/xml")]
		 [Consumes("application/xml")]
		 public IActionResult Post(DataDTO data){
		 	/*if(!preFlight(data.res_type)){
				//Find a more apropriate code for this
				return NotFound();
			}*/
		
			var mod = _context.Modules.SingleOrDefault(m => m.name == data.module);
			
			if(mod == null){
				return NotFound();
			}
			Console.WriteLine(mod.name);
			if(mod.parent.name != data.application){
				//TODO URGENT: Should this be an error or a warning?
			}
			/*
			Data datatosave = new Data(data);
			datatosave.parent=mod;
			//TODO: derefernce here... fix later
			mod.datas.Add(datatosave);

			_context.Add(datatosave);
			_context.SaveChanges();*/
			return Ok();//datatosave
		}
		[HttpPost("{module}")]
		public IActionResult Post(testeDTO data){
			Console.WriteLine(data.module);
			Console.WriteLine(data.bb);
			return Ok();
		}
	}
	public class testeDTO{
		[FromRoute]
		public string? module{get;set;}

		[FromBody]
		public string bb{get;set;}
		public testeDTO(string bb){
			
			this.bb = bb;
		}

	}
	public class DataDTO{
		
		[FromRoute]
		public string application { get; set; }
		[FromRoute]
		public string module { get; set; }
		[FromBody]
		public Data data { get; set; }
		
		public DataDTO(string application, string module, Data data){
			
			
			this.application = application;
			this.module = module;
			this.data = data;
		}
	}
}
