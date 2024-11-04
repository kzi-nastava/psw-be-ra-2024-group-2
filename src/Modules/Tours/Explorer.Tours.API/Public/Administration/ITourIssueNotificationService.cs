using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITourIssueNotificationService
    {
        Result<PagedResult<TourIssueNotificationDto>> GetPaged(int page, int pageSize);
        Result<TourIssueNotificationDto> Get(int notificationId);
        Result<PagedResult<TourIssueNotificationDto>> GetForUserId(long userId);
        Result<TourIssueNotificationDto> Create(TourIssueNotificationDto tourIssueNotificationDto);
        Result<TourIssueNotificationDto> Update(TourIssueNotificationDto tourIssueNotificationDto);
        void ReadNotifications(long userId, long tourIssueReportId);
        void ReadAllNotifications(long userId);
    }
}