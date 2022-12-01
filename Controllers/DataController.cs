using Microsoft.AspNetCore.Mvc;
using somiod.DAL;
using somiod.Models;

namespace somiod.Controllers{
    
    [ApiController]
	[Route("api/[controller]")]
	public class DataController: ControllerBase{
		private readonly InheritanceMappingContext _context;
		public DataController(InheritanceMappingContext context){
			_context = context;
		}
        //Through the REST API, it must be possible to create, modify, list, and delete each available resource. Data resources (records) and subscription resources only allow creation and deletion.
		
		//Create data
		[HttpPost]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Post([FromBody] Data data){
			_context.Add(data);
			_context.SaveChanges();
			return Ok(data);
		}
		//Delete data
		[HttpDelete("{id}")]
		[Produces("application/xml")]
		[Consumes("application/xml")]
		public IActionResult Delete(int id){
			var data = _context.Data.Find(id);
			if(data == null){
				return NotFound();
			}
			_context.Data.Remove(data);
			_context.SaveChanges();
			return Ok(data);
		}
	}
}
