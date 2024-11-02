using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Result<TourIssueNotificationDto> GetForUserId(long userId)
        {
            var list = repository.GetPaged(1, int.MaxValue).Results;
            List<TourIssueNotificationDto> dtos = new List<TourIssueNotificationDto>();
            dtos=dtos.Where(t=>t.ToUserId == userId).ToList();
            foreach (var d in list)
            {
                dtos.Add(MapToDto(d));
            }
            return dtos.FirstOrDefault() == null ? (Result<TourIssueNotificationDto>)Result.Ok() : Result.Ok(dtos.FirstOrDefault());
        }
        //useless
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
    }
}