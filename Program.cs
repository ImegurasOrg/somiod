using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using somiod.DAL;

namespace somiod{
	public class Program{
		//well it was good while it lasted... unfortunately we need custom stuff
		// public static void Main(string[] args){
		// 	var builder = WebApplication.(args);
			
			
			
		// 	var app = builder.Build();
		
		// 	Configure the HTTP request pipeline.
		// 	/*if (app.Environment.IsDevelopment())
		// 	{
		// 		app.UseSwagger();
		// 		app.UseSwaggerUI();
		// 	}*/
		// 	route / redirects to swagger
			
		// 	add cors permissive
		// 	app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
		
		// 	TOCHECK: is this needed? this might cause problems!
		// 	app.UseHttpsRedirection();

		// 	app.UseAuthorization();

		// 	app.MapControllers();

		// 	app.Run();
		// }
		public static string rootFolder = Environment.CurrentDirectory;
        static void Main(string[] args) {
			CreateHostBuilder(args).Build().Run();

        }
		public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
					
		});		
	}
}