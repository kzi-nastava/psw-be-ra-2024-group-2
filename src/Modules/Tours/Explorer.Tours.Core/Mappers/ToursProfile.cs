using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Payment.API.Dtos;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {

        //CreateMap<TourIssueCommentDto, TourIssueComment>()
        //    .ForMember(dest => dest.TourIssueReportId, opt => opt.MapFrom(src => src.TourIssueReportId.ToString()))
        //    .ForMember(dest => dest.Comment, opt => opt.MapFrom(dest => dest.Comment.ToString()))
        //    .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(dest => dest.PublishedAt.ToString()))
        //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(dest => dest.UserId.ToString()))
        //    .ForMember(dest => dest.Id, opt => opt.MapFrom(dest => dest.Id.ToString()));

        CreateMap<TourIssueCommentDto, TourIssueComment>().ReverseMap();

        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<Tour, TourDto>()
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => src.Equipment.Select(e => e.Id)))
            .ForMember(dest => dest.Checkpoints, opt => opt.MapFrom(src => src.Checkpoints.Select(e => e.Id)))
            .ForMember(dest => dest.TourDurationByTransportDtos, opt => opt.MapFrom(src => src.TourDurationByTransports));

        CreateMap<TourDurationByTransport, TourDurationByTransportDto>()
            .ForMember(dest => dest.Transport, opt => opt.MapFrom(src => src.TransportType.ToString()));

        CreateMap<TourIssueNotificationDto, TourIssueNotification>().ReverseMap();
        CreateMap<Checkpoint, CheckpointDto>()
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            //.ForMember(dest => dest.Tours, opt => opt.MapFrom(src => src.Tours));
            .ForMember(dest => dest.Secret, opt => opt.MapFrom(src => src.Secret));

        CreateMap<TourReview, TourReviewDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.TourId))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate))
                .ForMember(dest => dest.VisitDate, opt => opt.MapFrom(src => src.VisitDate))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));

        CreateMap<Image, TourImageDto>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.UploadedAt))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.GetMimeTypeNormalized));
        CreateMap<TourIssueReportDto, TourIssueReport>().ReverseMap();

        CreateMap<ObjectDto, TourObject>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ConstructUsing(src => new TourObject(src.Name, src.Description, Enum.Parse<ObjectCategory>(src.Category), src.Longitude, src.Latitude))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));

        CreateMap<Image, ObjectImageDto>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.UploadedAt))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.GetMimeTypeNormalized));

        CreateMap<ClubDto, Club>()
            .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId));
        CreateMap<ClubDto, Club>().ReverseMap();
        CreateMap<ClubInviteDTO,ClubInvite>().ReverseMap();

        CreateMap<TourPreference, TourPreferenceDto>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name)));

        CreateMap<TourPreferenceDto, TourPreference>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => new TourPreferenceTag(t))));

        CreateMap<TourExecution, TourExecutionDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.TourId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.SessionEndingTime, opt => opt.MapFrom(src => src.SessionEndingTime))
            .ForMember(dest => dest.LastActivity, opt => opt.MapFrom(src => src.LastActivity))
            .ForMember(dest => dest.tourExecutionCheckpoints,
                       opt => opt.MapFrom(src => src.TourExecutionCheckpoints));


        CreateMap<TourExecutionCheckpoint, TourExecutionCheckpointDto>()
            .ForMember(dest => dest.CheckpointId, opt => opt.MapFrom(src => src.CheckpointId))
            .ForMember(dest => dest.ArrivalAt, opt => opt.MapFrom(src => src.ArrivalAt));

        CreateMap<TourPaymentDto, TourDto>().ReverseMap();
        CreateMap<TourDurationByTransportPaymentDto, TourDurationByTransportDto>().ReverseMap();

        CreateMap<EventDto, Event>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EventAcceptances, opt => opt.MapFrom(src => src.EventAcceptances))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString())).ReverseMap();

        CreateMap<EventAcception, EventAcceptionDto>()
            .ForMember(dest => dest.TouristId, opt => opt.MapFrom(src => src.TouristId))
            .ForMember(dest => dest.AcceptedAt, opt => opt.MapFrom(src => src.AcceptedAt));


        CreateMap<Image, EventImageDto>()
         .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
         .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.UploadedAt))
         .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.GetMimeTypeNormalized));

        CreateMap<PersonalDairy, PersonalDairyDto>()
        .ForMember(dest => dest.chapters, opt => opt.MapFrom(src => src.Chapters))
        .ReverseMap();
        CreateMap<Chapter, ChapterDto>().ReverseMap();
        CreateMap<EventSubscription, EventSubscriptionDto>().ReverseMap();

        CreateMap<Chapter, ChapterDto>()
        .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
        .ReverseMap()
        .ForMember(dest => dest.Image, opt => opt.MapFrom(src => new Image(src.Image.Data, src.Image.UploadedAt, src.Image.MimeType)));

    }
}