using AutoMapper;
using EvacuateMe.BLL.DTO;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvacuateMe.BLL
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientRegisterDTO>();
            CreateMap<ClientRegisterDTO, Client>();

            CreateMap<LocationDTO, WorkerLocationHistory>();
            CreateMap<WorkerLocationHistory, LocationDTO>();

            CreateMap<LocationDTO, WorkerLastLocation>();
            CreateMap<WorkerLastLocation, LocationDTO>();

            CreateMap<Company, OrderCompanyDTO>();
            CreateMap<OrderCompanyDTO, Company>();

            CreateMap<Order, OrderCreateDTO>();
            CreateMap<OrderCreateDTO, Order>();

            CreateMap<Company, CompanyRegisterDTO>();
            CreateMap<CompanyRegisterDTO, Company>();
           
            CreateMap<Worker, WorkerRegisterDTO>();
            CreateMap<WorkerRegisterDTO, Worker>();
        }
    }
}
