using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain;
using System.Xml.Serialization;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<Tour, TourDto>()
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => src.Equipment.Select(e => e.Id)));
        CreateMap<Checkpoint, CheckpointDto>()
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));

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
        CreateMap<TourIssueReportDto, TourIssueReport>().ReverseMap();

        CreateMap<ObjectDto, TourObject>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ConstructUsing(src => new TourObject(src.Name, src.Description, Enum.Parse<ObjectCategory>(src.Category), src.Longitude, src.Latitude))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));


        CreateMap<Image, ObjectImageDto>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.UploadedAt))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.GetMimeTypeNormalized));

        CreateMap<ClubDto, Club>()
            .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId));
        CreateMap<ClubDto, Club>().ReverseMap();
        CreateMap<ClubInviteDTO,ClubInvite>().ReverseMap();
    }
   
    
}