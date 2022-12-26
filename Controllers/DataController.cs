using System.ComponentModel;
using System.Diagnostics;
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
		public IActionResult Post([FromRoute]string application, [FromRoute]string module){
			System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body);
			var body = reader.ReadToEndAsync();
			//Console.WriteLine(body.Result);
			XmlSerializer serializer = new XmlSerializer(typeof(DataDTO));
			//body.result to string
			System.IO.Stream str = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(body.Result));
			DataDTO parsedDataDTO = (DataDTO)serializer.Deserialize(str);
			Console.WriteLine(parsedDataDTO.content);
			return new ContentResult{
				ContentType = "application/xml",
				Content = "<done>ok</done>",
				StatusCode = 200

			};
			/*if(!preFlight(dataDTO.res_type)){
				//Find a more apropriate code for this
				return new ContentResult{
					ContentType = "application/xml",
					Content = "<error>Invalid resource type</error>",
					StatusCode = 400

				};
				
			}
			var mod = _context.Modules.SingleOrDefault(m => m.name == module);
			if(mod == null){
				return new ContentResult{
					ContentType = "application/xml",
					Content = "<error>No such module</error>",
					StatusCode = 400

				};
			}
			if(mod.parent.name != application){
				//TODO URGENT: Should this be an error or a warning?
			}

			Data datatosave = new Data(dataDTO.content);

			datatosave.parent=mod;
			_context.Add(datatosave);
			_context.SaveChanges();

			return new ContentResult{
				ContentType = "application/xml",
				Content = "<success>Resource created</success>",
				StatusCode = 200

			};
			
		
			/*
		
			Console.WriteLine(mod.name);
			
			/*
			Data datatosave = new Data(data);
			
			//TODO: derefernce here... fix later
			mod.datas.Add(datatosave);

			_context.Add(datatosave);
			_context.SaveChanges();*/
		}
		[HttpPost("{module}")]
		[Consumes("application/xml")]
		public IActionResult testePost([FromRoute] string module, [FromBody]teste teste){
			Console.WriteLine(teste.content);
			Console.WriteLine(module);
			//Microsoft.AspNetCore.Routing.RouteValueDictionary routeValues = Request.RouteValues;
			//string module = routeValues["module"].ToString();
			//Console.WriteLine("POST"+"a"+module);
			//Console.WriteLine("POST"+"a"+module);
			//parse body manually
			//System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body);
			//var body = reader.ReadToEndAsync();
			//Console.WriteLine(body.Result);

			return new ContentResult{
				ContentType = "application/xml",
				Content = "<done>Invalid resource type</done>",
				StatusCode = 200
			};
		}
		
		
	}
	public class teste{
		[FromBody]
		public string content { get; set; }
		public teste(string content){
			this.content = content;
		}
		public teste(){
			this.content = "default";
		}
	}

	public class DataDTO{
		public int id { get; set; }

		[DefaultValue("data")]
		public string res_type { get; set; }

		[DefaultValue("SampleData")]
        public string content { get; set; }

		private DateTime? creation_dt { get; set; }

        public DataDTO(int id, string content){
			this.id = id;
			this.res_type = Structures.res_type_str[(int)Structures.res_type.data];
			this.content = content;
			this.creation_dt = DateTime.Now;


		} 
		public DataDTO(){
			this.res_type = Structures.res_type_str[(int)Structures.res_type.data];
			this.creation_dt = DateTime.Now;
		}

		//XML Serializer needs a default constructor
		
	}
}
