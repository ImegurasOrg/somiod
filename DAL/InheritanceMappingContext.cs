
using Microsoft.AspNetCore.Mvc.Diagnostics;
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
		public DbSet<Data> Data { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }
		#pragma warning restore CS8618

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
			var config = new ConfigurationBuilder().AddJsonFile("appconfig.json", optional: false).Build();

			if (!optionsBuilder.IsConfigured)
			{
				
        		var serverVersion = ServerVersion.AutoDetect(config.GetConnectionString("myDbConn"));
				var sql = optionsBuilder.UseMySql(config.GetConnectionString("myDbConn"), serverVersion);
			
				
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			//APPLICATION 	
				//add default date to application
				modelBuilder.Entity<Application>().Property(a => a.creation_dt).HasDefaultValueSql("CURRENT_TIMESTAMP");
				//name is unique
				modelBuilder.Entity<Application>().HasIndex(a => a.name).IsUnique();
			//MODULE

				modelBuilder.Entity<Module>().Property(d => d.creation_dt).HasDefaultValueSql("CURRENT_TIMESTAMP");
				//cascade applications on removel 
				modelBuilder.Entity<Module>().HasIndex(m => m.name).IsUnique();

				modelBuilder.Entity<Module>().HasOne(m => m.parent).WithMany(a => a.modules).OnDelete(DeleteBehavior.Cascade);
				modelBuilder.Entity<Module>().Navigation(m=>m.parent).UsePropertyAccessMode(PropertyAccessMode.Property);

				
			//DATA
				//add default date to data
				modelBuilder.Entity<Data>().Property(d => d.creation_dt).HasDefaultValueSql("CURRENT_TIMESTAMP");

				//cascade data on removel 
				modelBuilder.Entity<Data>().HasOne(d => d.parent).WithMany(m => m.datas).OnDelete(DeleteBehavior.Cascade);
				modelBuilder.Entity<Data>().Navigation(d=>d.parent).UsePropertyAccessMode(PropertyAccessMode.Property);
			//SUBSCRIPTION
				//add default date to subscription
				modelBuilder.Entity<Subscription>().Property(s => s.creation_dt).HasDefaultValueSql("CURRENT_TIMESTAMP");
				//cascade subscription on removel 
				modelBuilder.Entity<Subscription>().HasOne(s => s.parent).WithMany(m => m.subscriptions).OnDelete(DeleteBehavior.Cascade);
				modelBuilder.Entity<Subscription>().Navigation(s=>s.parent).UsePropertyAccessMode(PropertyAccessMode.Property);

		}
	}
	
	
}
