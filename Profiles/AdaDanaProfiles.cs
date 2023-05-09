using AdaDanaService.Dtos;
using AdaDanaService.Models;
using AutoMapper;

namespace AdaDanaService.Profiles
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
