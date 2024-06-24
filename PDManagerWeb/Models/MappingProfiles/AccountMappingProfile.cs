using AutoMapper;
using PDManagerWeb.Models.DTOs;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace PDManagerWeb.Models.MappingProfiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<Account, AccountDTO>();
            CreateMap<AccountAuthDTO, Account>().
                ForMember(ac => ac.PasswordHash, p =>
                p.MapFrom(au => SHA256.HashData(Encoding.UTF8.GetBytes(au.Password)))).
                ForMember(ac => ac.Login, l =>
                l.MapFrom(au => au.Login.Trim()));
        }
    }
}
