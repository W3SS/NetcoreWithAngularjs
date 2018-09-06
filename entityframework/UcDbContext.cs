using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace entityframework
{
    public class UcDbContext :DbContext 
    {

        public UcDbContext():base()
        {

        }

        public UcDbContext(DbContextOptions<UcDbContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                //TODO entrpy
                //if (connectionString.Contains("data source"))
                //{
                    optionsBuilder.UseSqlServer(connectionString);
                //}

              
            }
            base.OnConfiguring(optionsBuilder);
            //var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //IConfigurationRoot config = builder.Build();
            //optionsBuilder.UseSqlServer(config.GetConnectionString("LibraryDemo1"));
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(
            //        @"Server=.;Database=MyDatabase;Trusted_Connection=True;");

            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("tUser");
            modelBuilder.Entity<User>().Property(c => c.Id).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(c => c.Name).HasMaxLength(255);
            modelBuilder.Entity<User>().Ignore(c => c.UniqueId);
            //modelBuilder.Entity<User>().Property(c => c.Gender);
            //modelBuilder.Entity<User>().Property(c => c.UniqueId).
            //modelBuilder.Ignore<User>();
            base.OnModelCreating(modelBuilder);

        }
    }
}
