using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace somiod.utils{
    // Thread Safe Singleton class
	public sealed class Structures{
		//java as a cool thing for once
		public  enum res_type {
			application=0,
			module=1,
			data=2, 
			subscription=3,
			error
		}
		
		public static readonly string[] res_type_str = new string[5] {
			"application",
			"module",
			"data",
			"subscription",
			"error"
		};
		public static string parse_res_type(res_type type){
			if((int)type>4||(int)type<0){
				return res_type_str[4];
			}
			return res_type_str[(int)type];
		}

    }
}
