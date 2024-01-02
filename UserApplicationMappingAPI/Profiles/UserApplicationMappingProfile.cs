using AutoMapper;
using UserApplicationMappingAPI.Models.DTO;
using UserApplicationMappingAPI.Model.Domain;

namespace UserAPI.Profiles
{
    public class UserApplicationMappingProfile : Profile
    {
        public UserApplicationMappingProfile()
        {
            CreateMap<UserApplicationMapping, UserApplicationMappingDto>().ReverseMap();
        }
    }
}
