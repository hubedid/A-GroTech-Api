using A_GroTech_Api.Dto;
using A_GroTech_Api.Models;
using AutoMapper;

namespace A_GroTech_Api.Helpers
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
        }
    }
}
