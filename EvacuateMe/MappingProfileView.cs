using AutoMapper;
using EvacuateMe.DAL.Entities;
using EvacuateMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvacuateMe
{
    public class MappingProfileVM : Profile
    {
        public MappingProfileVM()
        {
            CreateMap<Company, CompanyVM>();

            CreateMap<Worker, WorkerVM>();

            CreateMap<Order, OrderClientVM>();

            CreateMap<Order, CompletedOrderVM>();

            CreateMap<Order, OrderHistoryVM>();
        }
    }
}
