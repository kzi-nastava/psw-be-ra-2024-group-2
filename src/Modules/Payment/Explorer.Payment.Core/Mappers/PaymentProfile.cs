using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.Core.Domain;

namespace Explorer.Payment.Core.Mappers;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<TourOrderItemDto, TourOrderItem>().ReverseMap();
        CreateMap<TourPurchaseTokenDto,  TourPurchaseToken>().ReverseMap();

        // Map from TourBundle to BundleDto
        CreateMap<TourBundle, BundleDto>()
            .ForMember(dest => dest.Tours, opt => opt.MapFrom(src =>
                src.Tours.Select(tourId => new BundleItemDto
                {
                    TourId = tourId,
                    Price = 0, // Default value, update as needed
                    TourStatus = TourStatus.Draft // Default, update as needed
                }).ToList()))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        // Optional: Reverse map if needed
        CreateMap<BundleDto, TourBundle>()
            .ForMember(dest => dest.Tours, opt => opt.MapFrom(src =>
                src.Tours.Select(t => (int)t.TourId).ToList()))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId ?? 0))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ?? BundleStatus.Draft));
    }
}
