using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IProfileMessageNotificationService
    {
        Result<PagedResult<ProfileMessageNotificationDto>> GetForUserId(long userId);
        Result<ProfileMessageNotificationDto> Create(ProfileMessageNotificationDto messageNotificationDto);
        void ReadAllNotifications(long userId);
        void ReadNotification(long userId, long profileMessageNotificationId);
    }
}
