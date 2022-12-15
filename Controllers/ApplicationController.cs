using Microsoft.AspNetCore.Mvc;
using somiod.DAL;
using somiod.Models;

namespace somiod.Controllers{
	[ApiController]
	[Route("api/somiod/")]
	public class ApplicationController : ControllerBase{
		private readonly InheritanceMappingContext _context;
		public ApplicationController(InheritanceMappingContext context){
			_context = context;
		}
		
		//Get all applications
		[HttpGet]
		[Produces("application/xml")]
		public IActionResult Get(){
			var applications = _context.Aplications;
			return Ok(applications);
		}
		//Get application by id
		[HttpGet("{id}")]
		[Produces("application/xml")]
		public IActionResult Get(int id){
			var application = _context.Aplications.Find(id);
			return Ok(application);
		}
		//Create application
		[HttpPost]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Post([FromBody] Application application){
			_context.Aplications.Add(application);
			_context.SaveChanges();
			return Ok(application);
		}
		//Update application
		[HttpPut("{id}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Put(int id, [FromBody] Application application){
			_context.Aplications.Update(application);
			_context.SaveChanges();
			return Ok(application);
		}
		//Delete application
		[HttpDelete("{id}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Delete(int id){
			var application = _context.Aplications.Find(id);
			_context.Aplications.Remove(application);
			_context.SaveChanges();
			return Ok(application);
		}


	}
}
