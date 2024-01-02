using AutoMapper;
using ApplicationAPI.Models.Domain;
using ApplicationAPI.Models.DTO;

namespace UserAPI.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Application, ApplicationDto>().ReverseMap();
        }
    }
}
