using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap(); CreateMap<Tour, TourDto>()
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => src.Equipment.Select(e => e.Id)));
        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<ClubDto, Club>()
            .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        CreateMap<ClubDto, Club>().ReverseMap();
    }
}