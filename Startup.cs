using Microsoft.AspNetCore.Rewrite;
using somiod.Controllers;
using somiod.DAL;

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