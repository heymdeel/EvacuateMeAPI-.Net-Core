using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.DAL
{
    public class L2DBUnitOfWork : IUnitOfWork
    {
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

        public IRepository<Client> Clients
        {
            get
            {
                if (_clients == null)
                    _clients = new L2DBRepository<Client>();
                return _clients;
            }
        }

        public IRepository<Worker> Workers
        {
            get
            {
                if (_workers == null)
                    _workers = new L2DBRepository<Worker>();
                return _workers;
            }
        }

        public IRepository<Company> Companies
        {
            get
            {
                if (_companies == null)
                    _companies = new L2DBRepository<Company>();
                return _companies;
            }
        }

        public IRepository<SMSCode> SMSCodes
        {
            get
            {
                if (_smsCodes == null)
                    _smsCodes = new L2DBRepository<SMSCode>();
                return _smsCodes;
            }
        }

        public IRepository<Order> Orders
        {
            get
            {
                if (_orders == null)
                    _orders = new L2DBRepository<Order>();
                return _orders;
            }
        }

        public IRepository<WorkerLastLocation> WorkersLastLocation
        {
            get
            {
                if (_workersLastLocation == null)
                    _workersLastLocation = new L2DBRepository<WorkerLastLocation>();
                return _workersLastLocation;
            }
        }

        public IRepository<WorkerLocationHistory> WorkersLocationHistory
        {
            get
            {
                if (_workersLocationHistory == null)
                    _workersLocationHistory = new L2DBRepository<WorkerLocationHistory>();
                return _workersLocationHistory;
            }
        }

        public IRepository<CarType> CarTypes
        {
            get
            {
                if (_carTypes == null)
                    _carTypes = new L2DBRepository<CarType>();
                return _carTypes;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (_users == null)
                    _users = new L2DBRepository<User>();
                return _users;
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                if (_roles == null)
                    _roles = new L2DBRepository<Role>();
                return _roles;
            }
        }
    }
}
