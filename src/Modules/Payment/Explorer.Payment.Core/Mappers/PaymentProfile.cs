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

        CreateMap<TourWithPrice, BundleItemDto>().ReverseMap();
        CreateMap<BundleDto, TourBundle>().ReverseMap();
    }
}
