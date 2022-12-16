
using Microsoft.EntityFrameworkCore;
//Pomelo mysql
using Pomelo.EntityFrameworkCore.MySql;
using somiod.Models; 
using System.Configuration;
//using mysql drive
namespace somiod.DAL{
    public class InheritanceMappingContext : DbContext {
		//ignore warning(if this is null might as well crash the program)
		#pragma warning disable CS8618
		public DbSet<Application> Applications { get; set; }
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
			//add default date to application
			modelBuilder.Entity<Application>().Property(a => a.creation_dt).HasDefaultValueSql("CURRENT_TIMESTAMP");
			
		}
	}
	
	
}
