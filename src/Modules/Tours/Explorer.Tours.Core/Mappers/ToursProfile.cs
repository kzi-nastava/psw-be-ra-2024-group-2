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

        CreateMap<ObjectDto, TourObject>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ConstructUsing(src => new TourObject(src.Name, src.Description, Enum.Parse<ObjectCategory>(src.Category)))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));


        CreateMap<Image, ObjectImageDto>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.UploadedAt))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.GetMimeTypeNormalized));

        //CreateMap<EquipmentDto, Equipment>().ReverseMap();
        /*CreateMap<ObjectDto, TourObject>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
                src.Image != null
                    ? new Image(src.Image.Data, src.Image.UploadedAt, GetMimeTypeDenormalized(src.Image.MimeType))
                    : null))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<ObjectCategory>(src.Category)))
            .ForMember(dest => dest.ImageId, opt => opt.Ignore())
            .ConstructUsing(src => new TourObject(src.Name, src.Description, Enum.Parse<ObjectCategory>(src.Category)))
            .ReverseMap() // Automatically reverse the mapping to ObjectDto
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
                src.Image != null
                    ? new ObjectImageDto
                    {
                        Data = src.Image.Data,
                        UploadedAt = src.Image.UploadedAt,
                        MimeType = src.Image.GetMimeTypeNormalized
                    }
                    : null))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));*/

        /*CreateMap<ObjectImageDto, Image>()
            .ConstructUsing(src => new Image(src.Data, src.UploadedAt, GetMimeTypeDenormalized(src.MimeType)))
            .ReverseMap()
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.GetMimeTypeNormalized));*/ // Normalize the MIME type on reverse mapping
    }
    
}