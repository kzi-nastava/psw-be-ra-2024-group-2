﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITourIssueReportService
    {
        Result<PagedResult<TourIssueReportDto>> GetPaged(long userId, int page, int pageSize);
        //Result<TourIssueReportDto> Create(TourIssueReportDto report);
        Result<TourIssueReportDto> Create(long userId, long tourId, TourIssueReportDto tourIssueReport);
        Result<TourIssueReportDto> MarkAsDone(TourIssueReportDto tourIssueReport);
        Result<TourIssueReportDto> AlertNotDone(TourIssueReportDto tourIssueReport);
        Result<TourIssueReportDto> Get(int Id);
        public Result<TourIssueReportDto> SetFixUntilDate(TourIssueReportDto tourIssueReportDto, long fromUserId);
        public Result<TourIssueReportDto> CloseReport(TourIssueReportDto tourIssueReportDto);
    }
}
