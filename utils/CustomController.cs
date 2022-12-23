using System;
using Microsoft.AspNetCore.Mvc;

namespace somiod.utils
{
    public abstract class CustomController: ControllerBase{
		//true if we hit the right endpoint, else its not up to us
		public Structures.res_type res_type;
		[NonAction]
		public bool preFlight(string res_type){
			
			if(res_type == Structures.parse_res_type(this.res_type)){
				return true;
			}
			return false; 
		}
    }
}
