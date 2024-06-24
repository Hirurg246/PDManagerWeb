using AutoMapper;
using PDManagerWeb.Models.DTOs;
using System.Net;

namespace PDManagerWeb.Models.MappingProfiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<Account, AccountDTO>().ReverseMap();
        }
    }
}
