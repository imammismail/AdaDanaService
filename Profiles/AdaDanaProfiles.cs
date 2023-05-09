using AdaDanaApi.Dtos;
using AdaDanaApi.Models;
using AutoMapper;

namespace AdaDanaApi.Profiles
{
    public class AdaDanaProfiles : Profile
    {
        public AdaDanaProfiles()
        {
            // Source => destination
            CreateMap<User, ReadUserDto>();
            CreateMap<User, RegisterUserDto>();
            CreateMap<RegisterUserDto, User>();
        }
    }
}
