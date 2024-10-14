using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<Tour, TourDto>()
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => src.Equipment.Select(e => e.Id)));
    }
   
    
}