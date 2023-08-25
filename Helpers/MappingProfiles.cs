using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Models;
using AutoMapper;

namespace A_GroTech_Api.Helpers
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<Discussion, DiscussionDto>()
                .ForMember(dest => dest.CreatedBy, options => options.MapFrom(src => src.User));
			CreateMap<DiscussionPostDto, Discussion>();
			CreateMap<DiscussionPutDto, Discussion>();
			CreateMap<DiscussionAnswer, DiscussionAnswerDto>()
                .ForMember(dest => dest.AnsweredBy, options => options.MapFrom(src => src.AnsweredBy))
                .ForMember(dest => dest.DiscussionId, options => options.MapFrom(src => src.Discussion.Id));
			CreateMap<DiscussionAnswerPostDto, DiscussionAnswer>();
			CreateMap<DiscussionAnswerPutDto, DiscussionAnswer>();
			CreateMap<Area, AreaDto>();
            CreateMap<Image, ImageDto>();
			CreateMap<ImagePostDto, Image>();
        }
    }
}
