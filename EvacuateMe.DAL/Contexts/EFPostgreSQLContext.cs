using EvacuateMe.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace EvacuateMe.DAL.Contexts
{
    public class EFPostgreSQLContext : DbContext
    {
        public EFPostgreSQLContext() { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("User ID=ngmdcklcqvatps; Password=67483f8244dbd58058fe554618a87b33cd0dddfc5c7ad21fa3ace59c2e67c343; Server=ec2-79-125-125-97.eu-west-1.compute.amazonaws.com; Port=5432; Database=d3p9qhfg5eam1h; Pooling=true; SSL Mode=Require; Trust Server Certificate=true");

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new MyLoggerProvider());
            options.UseLoggerFactory(loggerFactory);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<SMSCode> SMSCodes { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<WorkerLastLocation> WorkerLastLocation { get; set; }
        public DbSet<WorkerLocationHistory> WorkersLocationHistory { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
