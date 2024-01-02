using AutoMapper;
using UserAPI.Models.DTO;
using UserAPI.Models.Domain;
using UserAPI.Models.Dto;

namespace UserAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
