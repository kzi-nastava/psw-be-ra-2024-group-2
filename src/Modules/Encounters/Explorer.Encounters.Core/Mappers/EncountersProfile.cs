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


        }
    }
}
