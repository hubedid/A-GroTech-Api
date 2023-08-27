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
            CreateMap<AreaPostDto, Area>();
            CreateMap<Image, ImageDto>();
			CreateMap<ImagePostDto, Image>();
            CreateMap<CommodityType, CommodityTypeDto>();
            CreateMap<CommodityTypePostDto, CommodityType>();
            CreateMap<Commodity, CommodityDto>();
            CreateMap<CommodityPostDto, Commodity>();
            CreateMap<CommodityPutDto, Commodity>();
            CreateMap<Prediction, PredictionDto>()
                .ForMember(dest => dest.CommodityId, options => options.MapFrom(src => src.Commodity.Id))
				.ForMember(dest => dest.AreaId, options => options.MapFrom(src => src.Area.Id));
            CreateMap<PredictionPostDto, Prediction>();
            CreateMap<Notification, NotificationDto>();
            CreateMap<NotificationPostDto, Notification>();
            CreateMap<NotificationPutDto, Notification>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CommodityId, options => options.MapFrom(src => src.Commodity.Id))
                .ForMember(dest => dest.AreaId, options => options.MapFrom(src => src.Area.Id))
                .ForMember(dest => dest.OwnerId, options => options.MapFrom(src => src.User.Id));
            CreateMap<ProductPostDto, Product>();
            CreateMap<ProductPutDto, Product>();
            CreateMap<ProductReview, ProductReviewDto>()
                .ForMember(dest => dest.ReviewedBy, options => options.MapFrom(src => src.ReviewedBy))
                .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.Id));
            CreateMap<ProductReviewPostDto, ProductReview>();
            CreateMap<ProductReviewPutDto, ProductReview>();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Buyer, options => options.MapFrom(src => src.Buyer))
                .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.Id));
        }
    }
}
