
using Microsoft.EntityFrameworkCore;
//Pomelo mysql
using Pomelo.EntityFrameworkCore.MySql;

using System.Configuration;
//using mysql drive
namespace somiod.DAL
{
    public class InheritanceMappingContext : DbContext {
		//public DbSet<Application> Aplications { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
			var connectionString = "";
        	var serverVersion = ServerVersion.AutoDetect(connectionString);
			optionsBuilder.UseMySql(connectionString, serverVersion);
			
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
		}
	}
	
	
}
