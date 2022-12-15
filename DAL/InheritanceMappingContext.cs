
using Microsoft.EntityFrameworkCore;
//Pomelo mysql
using Pomelo.EntityFrameworkCore.MySql;
using somiod.Models; 
using System.Configuration;
//using mysql drive
namespace somiod.DAL{
    public class InheritanceMappingContext : DbContext {
		public DbSet<Application> Aplications { get; set; }
		public DbSet<Module> Modules { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
			var config = new ConfigurationBuilder().AddJsonFile("appconfig.json", optional: false).Build();

			if (!optionsBuilder.IsConfigured)
			{
        		var serverVersion = ServerVersion.AutoDetect(config.GetConnectionString("myDbConn"));
				optionsBuilder.UseMySql(config.GetConnectionString("myDbConn"), serverVersion);
			
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
		}
	}
	
	
}
