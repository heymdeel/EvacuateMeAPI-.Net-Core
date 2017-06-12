using AutoMapper;
using EvacuateMe.BLL.DTO;
using EvacuateMe.BLL.DTO.Clients;
using EvacuateMe.BLL.DTO.CompanyDTO;
using EvacuateMe.BLL.DTO.Orders;
using EvacuateMe.BLL.DTO.Workers;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvacuateMe.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientSignUpDTO>();
            CreateMap<ClientSignUpDTO, Client>();

            CreateMap<Order, OrderClientDTO>();
            CreateMap<OrderClientDTO, Order>();

            CreateMap<LocationDTO, WorkerLocationHistory>();
            CreateMap<WorkerLocationHistory, LocationDTO>();

            CreateMap<LocationDTO, WorkerLastLocation>();
            CreateMap<WorkerLastLocation, LocationDTO>();

            CreateMap<Company, OrderCompanyDTO>();
            CreateMap<OrderCompanyDTO, Company>();

            CreateMap<Order, OrderCreateDTO>();
            CreateMap<OrderCreateDTO, Order>();

            CreateMap<Order, CompletedOrderDTO>();
            CreateMap<CompletedOrderDTO, Order>();

            CreateMap<Order, OrderHistoryDTO>();
            CreateMap<OrderHistoryDTO, Order>();

            CreateMap<Company, CompanyDTO>();
            CreateMap<CompanyDTO, Company>();

            CreateMap<Company, CompanyRegisterDTO>();
            CreateMap<CompanyRegisterDTO, Company>();

            CreateMap<Worker, WorkerDTO>();
            CreateMap<WorkerDTO, Worker>();

            CreateMap<Worker, WorkerSignUpDTO>();
            CreateMap<WorkerSignUpDTO, Worker>();
        }
    }
}
