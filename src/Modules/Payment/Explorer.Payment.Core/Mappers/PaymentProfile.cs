using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.Core.Domain;

namespace Explorer.Payment.Core.Mappers;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<TourOrderItemDto, TourOrderItem>().ReverseMap();
        CreateMap<TourPurchaseTokenDto, TourPurchaseToken>().ReverseMap();
        CreateMap<TourPurchaseToken, PurchaseToken>().ReverseMap();
        CreateMap<BundleOrderItemDto, BundleOrderItem>().ReverseMap();
        CreateMap<SouvenirOrderItemDto, SouvenirOrderItem>().ReverseMap();

        CreateMap<WalletDto, Wallet>().ReverseMap();

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

        CreateMap<TourSaleTour, TourDtoPayment>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TourId))
            .ReverseMap();

        CreateMap<TourSale, TourSaleDto>()
            .ForMember(dest => dest.Tours, opt => opt.MapFrom(src => src.Tours))
            .ReverseMap();

        CreateMap<Coupon, CouponDto>().ReverseMap();

        CreateMap<TourSouvenir, TourSouvenirDto>()
            .ForMember(dest => dest.ImageDto, opt => opt.MapFrom(src => src.Image));

        CreateMap<Image, PaymentImageDto>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.UploadedAt))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.GetMimeTypeNormalized));
    }
}
