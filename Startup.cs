using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using somiod.Controllers;
using somiod.DAL;
using somiod.utils;
namespace somiod{
	public class Startup {
		public Startup(IConfiguration configuration){
			Configuration = configuration;
		}
		public IConfiguration Configuration { get; }
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services){
		
			// Add services to the container.
			services.AddDbContext<InheritanceMappingContext>();
			
			//quick plug on the enabling of xml stuff
			services.AddControllers().AddXmlSerializerFormatters();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			
			
			services.AddCors(options => {
				options.AddPolicy("AllowAll", builder => {
					builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader();
				});
			});
		}
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
			if (env.IsDevelopment()){
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			var option = new RewriteOptions();
			option.AddRedirect("^$", "swagger");
			app.UseRewriter(option);

			app.UseCors("AllowAll");
			app.Use(async (context, next) => {
				
				//if the path of the url is domain/api/somiod/{application}/{module} then we need to parse the body
				//and send it to the correct controller
				//Console.WriteLine(context.Request.Path.Value);
				string pattern = @"(\/api\/somiod)\/([a-zA-Z\-_0-9]+)\/([a-zA-Z\-_0-9]+)\/?$";
				try{
					//rewindable body
					context.Request.EnableBuffering();
					#pragma warning disable //the day context.Request.Path.Value is null is the day asp.net becomes obsolete
					Match match = Regex.Match(context.Request.Path.Value, pattern);
					#pragma warning restore
					if(match.Success){
						//Check if body contains <res_type>subscription</res_type>
						//if so redirect to the subscription controller
						//else dont redirect
						
						if(context.Request.Body != null){
							//Console.WriteLine("Body is not null");
							//Console.WriteLine(context.Request.Body);
							
							using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true)){
								var outOfVarnames= await reader.ReadToEndAsync(); 
								Match match2 = Regex.Match(outOfVarnames, @"<res_type>subscription<\/res_type>");
								if(match2.Success){
									
									context.Request.Path= match.Groups[1].Value+"/Subscription/"+match.Groups[2].Value+"/"+match.Groups[3].Value;
									
								}
								//rewind the body stream so the controller can read it
								context.Request.Body.Position = 0;

							}
						}

					}
				}catch(NullReferenceException e){
					Console.WriteLine("O valor do path é nulo"+ e.Message);
					//TODO GIVE A BAD REQUEST
					//return a bad request
					return; 
				}
				await next();
			});
			app.UseRouting();
			
			app.UseAuthorization();
			app.UseEndpoints(endpoints => {
				
				endpoints.MapControllers();
				/*endpoints.MapControllerRoute(
					name: "postDataCustom",
					pattern: "api/somiod/{application}/{module}", new {controller = "DataController", action = "Post"});
				*/
			});
		}
	}	
	
}