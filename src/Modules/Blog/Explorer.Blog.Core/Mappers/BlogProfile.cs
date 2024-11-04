using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Blog.Core.Mappers;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        /*
        CreateMap<BlogDto, Explorer.Blog.Core.Domain.Blog>()
        .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
        .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
        .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
        .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
        .ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => src.Ratings));

        CreateMap<Core.Domain.Blog, BlogDto>()
        .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
        .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
        .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
        .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
        .ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => src.Ratings));


        CreateMap<CommentDTO, Comment>();
        CreateMap<RatingDto, Rating>();

        CreateMap<Rating, RatingDto>()
           .ForMember(dest => dest.RatingType, opt => opt.MapFrom(src => src.RatingType.ToString()))
           .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
           .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
        */

        CreateMap<BlogDto, Explorer.Blog.Core.Domain.Blog>().ReverseMap();
        CreateMap<CommentDTO, Comment>().ReverseMap();
        CreateMap<Rating, RatingDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt)) // Mapiranje datuma
            .ReverseMap()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt)); // Mapiranje unazad
        CreateMap<BlogDto, Explorer.Blog.Core.Domain.Blog>().IncludeAllDerived()
            .ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => src.Ratings.Select((rating) => new Rating(Enum.Parse<RatingType>(rating.RatingType),rating.CreatedAt, rating.Username))));


    }
}