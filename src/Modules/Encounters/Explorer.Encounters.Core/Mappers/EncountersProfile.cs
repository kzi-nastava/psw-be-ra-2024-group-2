using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Mappers
{
    public class EncountersProfile : Profile
    {
        public EncountersProfile()
        {

            CreateMap<Encounter, EncounterDto>().ReverseMap();

            CreateMap<SocialEncounter, SocialEncounterDto>().ReverseMap();

            CreateMap<HiddenLocationEncounter, HiddenLocationEncounterDto>().ReverseMap();

            CreateMap<MiscEncounter, MiscEncounterDto>().ReverseMap();

            CreateMap<Image, EncounterImageDto>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.UploadedAt))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.GetMimeTypeNormalized));

            CreateMap<UserLevel, UserLevelDto>().ReverseMap();

            CreateMap<UnifiedEncounterDto, SocialEncounter>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? string.Empty))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
            .ForMember(dest => dest.Lattitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.RequiredPeople, opt => opt.MapFrom(src => src.RequiredPeople.HasValue ? src.RequiredPeople.Value : 0))
            .ForMember(dest => dest.RangeInMeters, opt => opt.MapFrom(src => src.SocialRangeInMeters.HasValue ? src.SocialRangeInMeters.Value : 0))
            .ForMember(dest => dest.TouristsInRange, opt => opt.MapFrom(src => src.TouristsInRange ?? new List<int>())) 
            .ReverseMap();

            CreateMap<UnifiedEncounterDto, Encounter>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Lattitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
            .ReverseMap();

            CreateMap<UnifiedEncounterDto, HiddenLocationEncounter>()
                .IncludeBase<UnifiedEncounterDto, Encounter>()
                .ForMember(dest => dest.Image, opt => opt.Ignore()) // Assuming Image requires separate mapping
                .ForMember(dest => dest.TargetLatitude, opt => opt.MapFrom(src => src.TargetLatitude))
                .ForMember(dest => dest.TargetLongitude, opt => opt.MapFrom(src => src.TargetLongitude))
                .ForMember(dest => dest.RangeInMeters, opt => opt.MapFrom(src => src.HiddenLocationRangeInMeters))
                .ReverseMap();

            CreateMap<UnifiedEncounterDto, MiscEncounter>()
                .IncludeBase<UnifiedEncounterDto, Encounter>()
                .ForMember(dest => dest.ActionDescription, opt => opt.MapFrom(src => src.ActionDescription))
                .ReverseMap();

            CreateMap<UnifiedEncounterDto, SocialEncounter>()
                .IncludeBase<UnifiedEncounterDto, Encounter>()
                .ForMember(dest => dest.RequiredPeople, opt => opt.MapFrom(src => src.RequiredPeople))
                .ForMember(dest => dest.RangeInMeters, opt => opt.MapFrom(src => src.SocialRangeInMeters))
                .ForMember(dest => dest.TouristsInRange, opt => opt.MapFrom(src => src.TouristsInRange))
                .ReverseMap();
        }
    }
}
