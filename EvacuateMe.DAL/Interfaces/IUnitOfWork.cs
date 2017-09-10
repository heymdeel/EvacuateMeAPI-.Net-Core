using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Client> Clients { get; }
        IRepository<Worker> Workers { get; }
        IRepository<Company> Companies { get; }
        IRepository<Order> Orders { get; }
        IRepository<SMSCode> SMSCodes { get; }
        IRepository<WorkerLastLocation> WorkersLastLocation { get; }
        IRepository<WorkerLocationHistory> WorkersLocationHistory { get; }
        IRepository<CarType> CarTypes { get; }
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
    }
}
