using AutoMapper;
using EvacuateMe.BLL.DTO;
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

            CreateMap<Order, OrderInfoDTO>();
            CreateMap<OrderInfoDTO, Order>();
        }
    }
}
