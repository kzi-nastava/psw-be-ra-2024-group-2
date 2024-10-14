using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<Tour, TourDto>()
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => src.Equipment.Select(e => e.Id)));

        CreateMap<TourReview, TourReviewDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.TourId))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate))
                .ForMember(dest => dest.VisitDate, opt => opt.MapFrom(src => src.VisitDate))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));


        CreateMap<Image, TourImageDto>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.UploadedAt))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.GetMimeTypeNormalized));
        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<TourIssueReportDto, TourIssueReport>().ReverseMap();
    }
   
    
}