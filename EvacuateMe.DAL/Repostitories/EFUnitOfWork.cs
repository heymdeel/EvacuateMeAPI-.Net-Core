using EvacuateMe.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using EvacuateMe.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvacuateMe.DAL.Repostitories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private EFPostgreSQLContext db;
        private IRepository<Client> _clients;
        private IRepository<Worker> _workers;
        private IRepository<Company> _companies;
        private IRepository<Order> _orders;
        private IRepository<SMSCode> _smsCodes;
        private IRepository<WorkerLocationHistory> _workersLocationHistory;
        private IRepository<WorkerLastLocation> _workersLastLocation;
        private IRepository<CarType> _carTypes;
        private IRepository<User> _users;
        private IRepository<Role> _roles;

        public EFUnitOfWork()
        {
            db = new EFPostgreSQLContext();
        }

        public IRepository<Client> Clients
        {
            get
            {
                if (_clients == null)
                    _clients = new EFGenericRepository<Client>(db);
                return _clients;
            }
        }

        public IRepository<Worker> Workers
        {
            get
            {
                if (_workers == null)
                    _workers = new EFGenericRepository<Worker>(db);
                return _workers;
            }
        }

        public IRepository<Company> Companies
        {
            get
            {
                if (_companies == null)
                    _companies = new EFGenericRepository<Company>(db);
                return _companies;
            }
        }

        public IRepository<SMSCode> SMSCodes
        {
            get
            {
                if (_smsCodes == null)
                    _smsCodes = new EFGenericRepository<SMSCode>(db);
                return _smsCodes;
            }
        }

        public IRepository<Order> Orders
        {
            get
            {
                if (_orders == null)
                    _orders = new EFGenericRepository<Order>(db);
                return _orders;
            }
        }

        public IRepository<WorkerLastLocation> WorkersLastLocation
        {
            get
            {
                if (_workersLastLocation == null)
                    _workersLastLocation = new EFGenericRepository<WorkerLastLocation>(db);
                return _workersLastLocation;
            }
        }

        public IRepository<WorkerLocationHistory> WorkersLocationHistory
        {
            get
            {
                if (_workersLocationHistory == null)
                    _workersLocationHistory = new EFGenericRepository<WorkerLocationHistory>(db);
                return _workersLocationHistory;
            }
        }

        public IRepository<CarType> CarTypes
        {
            get
            {
                if (_carTypes == null)
                    _carTypes = new EFGenericRepository<CarType>(db);
                return _carTypes;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (_users == null)
                    _users = new EFGenericRepository<User>(db);
                return _users;
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                if (_roles == null)
                    _roles = new EFGenericRepository<Role>(db);
                return _roles;
            }
        }
    }
}
