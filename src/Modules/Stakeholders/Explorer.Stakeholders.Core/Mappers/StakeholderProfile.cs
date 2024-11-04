using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {
        CreateMap<Person, ProfileDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography))
            .ForMember(dest => dest.Moto, opt => opt.MapFrom(src => src.Moto))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));

        CreateMap<Image, ImageDto>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.UploadedAt))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.GetMimeTypeNormalized));


        CreateMap<RatingApplicationDto, RatingApplication>().ReverseMap();
        // Mapping for TouristPosition
        CreateMap<TouristPosition, TouristPositionDto>()
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));

        CreateMap<Person, PersonDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.TouristPosition, opt => opt.MapFrom(src => src.TouristPosition));


        CreateMap<Person, AccountDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int)src.User.Role))
            .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(src => src.User.IsBlocked));

    }
}