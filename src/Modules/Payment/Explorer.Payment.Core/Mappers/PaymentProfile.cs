using AutoMapper;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.Core.Domain;

namespace Explorer.Payment.Core.Mappers;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<OrderItemDto, OrderItem>().ReverseMap();
        CreateMap<TourPurchaseTokenDto,  TourPurchaseToken>().ReverseMap();

        CreateMap<TourSaleTour, TourDtoPayment>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TourId))
            .ReverseMap();

        CreateMap<TourSale, TourSaleDto>()
            .ForMember(dest => dest.Tours, opt => opt.MapFrom(src => src.Tours))
            .ReverseMap();
    }
}
