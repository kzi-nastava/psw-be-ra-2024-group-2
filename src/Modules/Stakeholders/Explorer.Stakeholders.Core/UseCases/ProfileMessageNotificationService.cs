using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class ProfileMessageNotificationService : CrudService<ProfileMessageNotificationDto, ProfileMessageNotification>, IProfileMessageNotificationService
    {
        ICrudRepository<ProfileMessageNotification> repository;
        ITransactionRepository transactionRepository;
        public ProfileMessageNotificationService(ICrudRepository<ProfileMessageNotification> messageNotificationRepository, ITransactionRepository transactionRepository, IMapper mapper) : base(messageNotificationRepository, mapper)
        {
            repository = messageNotificationRepository;
            this.transactionRepository = transactionRepository;
        }
        Result<ProfileMessageNotificationDto> Create(ProfileMessageNotificationDto messageNotificationDto)
        {
            var list = repository.GetPaged(1, int.MaxValue).Results;
            List<ProfileMessageNotificationDto> dtos = new List<ProfileMessageNotificationDto>();
            ProfileMessageNotificationDto dto = dtos.Where(t => t.SenderId == messageNotificationDto.SenderId).ToList().FirstOrDefault() ?? null;
            if (dto != null && dto.Status == ProfileMessageNotificationStatus.Unread)
            {
                return MapToDto(repository.Update(MapToDomain(messageNotificationDto)));
            }
            var newDto = MapToDto(repository.Create(MapToDomain(messageNotificationDto)));
            return newDto;
        }

        public Result<PagedResult<ProfileMessageNotificationDto>> GetForUserId(long userId)
        {
            List<ProfileMessageNotification> list = repository.GetPaged(1, int.MaxValue).Results;
            List<ProfileMessageNotificationDto> dtos = list.Select(obj => MapToDto(obj)).ToList();
            dtos = dtos.Where(t => t.Status == ProfileMessageNotificationStatus.Unread).ToList();
            dtos = dtos.Where(t => t.RecipientId == userId).ToList();
            var result = new PagedResult<ProfileMessageNotificationDto>(dtos.ToList(), dtos.Count);
            return result;
        }

        public void ReadAllNotifications(long userId)
        {
            var notifications = repository.GetPaged(1, int.MaxValue).Results;
            foreach (var n in notifications)
            {
                if (n.RecipientId == userId)
                {
                    n.Read();
                    repository.Update(n);
                }
            }
        }

        public void ReadNotification(long userId, long profileMessageNotificationId)
        {
            var notifications = repository.GetPaged(1, int.MaxValue).Results;
            foreach (var n in notifications)
            {
                if (n.RecipientId == userId && n.Id == profileMessageNotificationId)
                {
                    n.Read();
                    repository.Update(n);
                }
            }
        }
    }
}
