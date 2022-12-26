using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

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
		[Consumes("application/xml")]
		[Produces("application/xml")]
		public IActionResult Post([FromRoute]string application, [FromRoute]string module, [FromBody]DataDTO dataDTO){
			if(!preFlight(dataDTO.res_type)){
				//Find a more apropriate code for this
				return UnprocessableEntity();
			}
			var mod = _context.Modules.SingleOrDefault(m => m.name == module);
			//TODO: mod?.parent?.name != application
			if(mod == null){

				return UnprocessableEntity();
			}
			Data data = dataDTO.fromDTO();
			data.parent=mod;
			_context.Add(data);
			_context.SaveChanges();
			return Ok();
		}
		
		
	}

	public class DataDTO{

		public int? id { get; set; }

		[DefaultValue("data")]
		public string res_type { get; set; }

		[DefaultValue("SampleData")]
        public string content { get; set; }

		private DateTime? creation_dt { get; set; }

        public DataDTO(string content){
			this.res_type = Structures.res_type_str[(int)Structures.res_type.data];
			this.content = content;
			this.creation_dt = DateTime.Now;


		} 
		public DataDTO(string content, int id):this(content){
			this.id = id;

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
