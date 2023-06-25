using AccountManager.Domain.Dto;
using AccountManager.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Application
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            //Maping for user and registerUser dto
            CreateMap<AppUser, RegisterRequestDTO>().ReverseMap();
            CreateMap<AppUser, RegisterResponseDTO>().ReverseMap();
            CreateMap<AppUser, UserResponseDTO>().ReverseMap();
        }
    }
}
