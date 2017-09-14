using EvacuateMe.DAL.Entities;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.DAL.Contexts
{
    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }

    public class MySettings : ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders
        {
            get { yield break; }
        }

        public string DefaultConfiguration => "Npgsql";
        public string DefaultDataProvider => "Npgsql";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = "Kek",
                        ProviderName = "Npgsql",
                        ConnectionString = "User ID=ngmdcklcqvatps; Password=67483f8244dbd58058fe554618a87b33cd0dddfc5c7ad21fa3ace59c2e67c343; Server=ec2-79-125-125-97.eu-west-1.compute.amazonaws.com; Port=5432; Database=d3p9qhfg5eam1h; Pooling=true; SSL Mode=Require; Trust Server Certificate=true"
                    };
            }
        }
    }

    public class L2DBContext : DataConnection
    {
        static L2DBContext()
        {
            DefaultSettings = new MySettings();
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
        }

        public L2DBContext() : base("Kek") { }

        public ITable<Client> Clients { get => GetTable<Client>(); }
        public ITable<SMSCode> SMSCodes { get => GetTable<SMSCode>(); }
        public ITable<Worker> Workers { get => GetTable<Worker>(); }
        public ITable<Company> Companies { get => GetTable<Company>(); }
        public ITable<WorkerLastLocation> WorkerLastLocation { get => GetTable<WorkerLastLocation>(); }
        public ITable<WorkerLocationHistory> WorkersLocationHistory { get => GetTable<WorkerLocationHistory>(); }
        public ITable<Order> Orders { get => GetTable<Order>(); }
        public ITable<CarType> CarTypes { get => GetTable<CarType>(); }
        public ITable<User> Users { get => GetTable<User>(); }
        public ITable<Role> Roles { get => GetTable<Role>(); }
    }
}
