using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourIssueNotificationService : CrudService<TourIssueNotificationDto, TourIssueNotification>, ITourIssueNotificationService
    {
        ICrudRepository<TourIssueNotification> repository;
        ITransactionRepository transactionRepository;
        public TourIssueNotificationService(ICrudRepository<TourIssueNotification> tourIssueRepository,ITransactionRepository transactionRepository, IMapper mapper) : base(tourIssueRepository, mapper)
        {
            repository = tourIssueRepository;
            this.transactionRepository = transactionRepository;
        }
        Result<TourIssueNotificationDto> Create(TourIssueNotificationDto tourIssueNotificationDto)
        {
            var list = repository.GetPaged(1, int.MaxValue).Results;
            List<TourIssueNotificationDto> dtos = new List<TourIssueNotificationDto>();
            TourIssueNotificationDto dto = dtos.Where(t => t.ToUserId == tourIssueNotificationDto.ToUserId).ToList().FirstOrDefault() ?? null;
            if (dto != null && dto.Status==TourIssueNotificationStatus.Unread)
            {
                var id = dto.Id;
                tourIssueNotificationDto.Id = id;
                return MapToDto(repository.Update(MapToDomain(tourIssueNotificationDto)));
            }
            var newDto = MapToDto( repository.Create(MapToDomain(tourIssueNotificationDto)));
            return newDto;
        }
        public Result<PagedResult<TourIssueNotificationDto>> GetForUserId(long userId)
        {
            List<TourIssueNotification> list = repository.GetPaged(1, int.MaxValue).Results;
            List<TourIssueNotificationDto> dtos = list.Select(obj=>MapToDto(obj)).ToList();
            dtos = dtos.Where(t => t.Status == TourIssueNotificationStatus.Unread).ToList();
            dtos= dtos.Where(t=>t.ToUserId == userId).ToList();
            var result = new PagedResult<TourIssueNotificationDto>(dtos.ToList(), dtos.Count);
            return result;
        }
        public Result<TourIssueNotificationDto> MarkAsOpened(TourIssueNotificationDto dto) {
            var notification = repository.Get(dto.Id);
            if (notification != null)
            {
                return Result.Fail(FailureCode.Conflict).WithError("This notification doesn't exist");
            }
            var notif = MapToDomain(dto);
            notif.Read();
            repository.Update(notif);
            return Result.Ok(MapToDto(notif));
        }
        //Create

        Result<TourIssueNotificationDto> ITourIssueNotificationService.Create(TourIssueNotificationDto dto)
        {
            try
            {
                var notif = new TourIssueNotification(dto.FromUserId, dto.ToUserId, dto.Status, dto.TourIssueReportId);
                repository.Create(notif);
                return MapToDto(notif);
            }
            catch(Exception e)
            {
                return Result.Fail(FailureCode.Conflict).WithError("Error creating the tour.");
            }
        }
        public void ReadNotifications(long userId, long tourIssueReportId)
        {
            var notifications = repository.GetPaged(1, int.MaxValue).Results;
            foreach(var n in notifications)
            {
                if(n.ToUserId==userId && n.TourIssueReportId == tourIssueReportId)
                {
                    n.Read();
                    repository.Update(n);
                }
            }
        }
        public void ReadAllNotifications(long userId)
        {
            var notifications = repository.GetPaged(1, int.MaxValue).Results;
            foreach (var n in notifications)
            {
                if (n.ToUserId == userId)
                {
                    n.Read();
                    repository.Update(n);
                }
            }
        }
    }
}