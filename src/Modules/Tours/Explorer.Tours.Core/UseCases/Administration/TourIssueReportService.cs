using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
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
    public class TourIssueReportService : CrudService<TourIssueReportDto, TourIssueReport>, ITourIssueReportService
    {
        private readonly ICrudRepository<TourIssueReport> _tourIssueReportRepository;
        private readonly ICrudRepository<Tour> _tourRepository;
        private readonly ICrudRepository<TourIssueComment> _tourIssueCommentRepository;
        private readonly ICrudRepository<TourIssueNotification> _tourIssueNotificationRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICrudRepository<User> _userRepository;
        public TourIssueReportService(ICrudRepository<TourIssueReport> repository, ICrudRepository<Tour> tourRepository, 
            ICrudRepository<TourIssueComment> tourIssueCommentRepository, ITransactionRepository transactionRepository, ICrudRepository<TourIssueNotification> tourIssueNotificationRepository, ICrudRepository<User> userRepository, IMapper mapper) : base(repository, mapper)
        {
            _tourIssueReportRepository = repository;
            _tourRepository = tourRepository;
            _tourIssueCommentRepository = tourIssueCommentRepository;
            _transactionRepository = transactionRepository;
            _tourIssueNotificationRepository = tourIssueNotificationRepository;
            _userRepository = userRepository;
        }

        public Result<PagedResult<TourIssueReportDto>> GetPaged(long userId, int page, int pageSize)
        {
            List<TourIssueReportDto> tourIssueReportDtos = new List<TourIssueReportDto>();
            User user = _userRepository.Get(userId);
            var allTourIssueReports = _tourIssueReportRepository.GetPaged(page, pageSize);
            List<TourIssueReport> reports = new List<TourIssueReport>();

            if(user.Role == UserRole.Tourist)
            {
                reports = allTourIssueReports.Results.Where(t => t.UserId == userId).ToList();
            }
            else if(user.Role == UserRole.Author)
            {
                foreach(TourIssueReport report in allTourIssueReports.Results)
                {
                    Tour tour = _tourRepository.Get(report.TourId);
                    if (tour.UserId == userId)
                        reports.Add(report);
                }
            }
            else if(user.Role == UserRole.Administrator)
            {
                reports = allTourIssueReports.Results;
            }
            foreach(var rep in reports)
                tourIssueReportDtos.Add(MapToDto(rep));

            return new PagedResult<TourIssueReportDto>(tourIssueReportDtos, tourIssueReportDtos.Count());
        }
        public Result<TourIssueReportDto> Create(long userId, long tourId, TourIssueReportDto tourIssueReport)
        {
            try
            {
                _transactionRepository.BeginTransaction();
                var tour = _tourRepository.Get(tourId);
                var pagedResult = base.GetPaged(1, int.MaxValue);
                if (pagedResult.IsFailed)
                {
                    return Result.Fail(pagedResult.Errors);
                }
                var filteredReviews = pagedResult.Value.Results
                    .Any(r => r.TourId == tourId && r.UserId == userId);
                if (filteredReviews)
                {
                    return Result.Fail("A report for this user and tour already exists.");
                }
                var tourIssue = MapToDomain(tourIssueReport);
                var results = _tourIssueReportRepository.Create(tourIssue);
                _transactionRepository.CommitTransaction();
                return MapToDto(results);
            }
            catch(Exception e) 
            {

                _transactionRepository.RollbackTransaction();
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<TourIssueReportDto> SetFixUntilDate(TourIssueReportDto tourIssueReportDto, long fromUserId) //fromUserId is admin in this case
        {
            try
            {
                _transactionRepository.BeginTransaction();
                TourIssueReport tourIssueReport = MapToDomain(tourIssueReportDto);
                var result = _tourIssueReportRepository.Update(tourIssueReport);
                Tour tour = _tourRepository.Get(tourIssueReport.TourId);
                long toUserId = tour.UserId;
                TourIssueNotification notification = new TourIssueNotification(fromUserId, toUserId, 
                TourIssueNotificationStatus.Unread, tourIssueReport.Id);
                _tourIssueNotificationRepository.Create(notification);
                _transactionRepository.CommitTransaction();
                return MapToDto(result);
            }
            catch (Exception e)
            {
                _transactionRepository.RollbackTransaction();
                return Result.Fail(FailureCode.Conflict).WithError(e.Message);
            }
        }
        public Result<TourIssueReportDto> CloseReport(TourIssueReportDto tourIssueReportDto)
        {
            try
            {
                _transactionRepository.BeginTransaction();
                TourIssueReport tourIssue = _tourIssueReportRepository.Get(tourIssueReportDto.Id);
                tourIssue.CloseReport();
                _tourIssueReportRepository.Update(tourIssue);
                _transactionRepository.CommitTransaction();
                return MapToDto(tourIssue);
            }
            catch (Exception e)
            {
                _transactionRepository.RollbackTransaction();
                return Result.Fail(FailureCode.Conflict).WithError(e.Message);
            }
        }
        public Result<TourIssueReportDto> MarkAsDone(TourIssueReportDto tourIssueReportDto)
        {
            try
            {
                _transactionRepository.BeginTransaction();
                TourIssueReport tourIssueReport = _tourIssueReportRepository.Get(tourIssueReportDto.Id);
                tourIssueReport.MarkAsDone();
                _tourIssueReportRepository.Update(tourIssueReport);
                _transactionRepository.CommitTransaction();
                return MapToDto(tourIssueReport);
            }
            catch (Exception e)
            {
                _transactionRepository.RollbackTransaction();
                return Result.Fail(FailureCode.Conflict).WithError(e.Message);
            }
        }

        public Result<TourIssueReportDto> AlertNotDone(TourIssueReportDto tourIssueReportDto)
        {
            try
            {
                _transactionRepository.BeginTransaction();
                var tourIssue = MapToDomain(tourIssueReportDto);
                var tourIssueGet = _tourIssueReportRepository.Get(tourIssue.Id) ?? throw new Exception();
                tourIssueGet.AlertNotDone();
                var results = _tourIssueReportRepository.Update(tourIssueGet);
                _transactionRepository.CommitTransaction();
                return MapToDto(results);
            }
            catch (Exception e)
            {
                _transactionRepository.RollbackTransaction();
                return Result.Fail(FailureCode.Conflict).WithError(e.Message);
            }
        }
        public Result<TourIssueReportDto> ReadNotification(TourIssueReportDto tourIssueReportDto,long tourNotificationId)
        {
            try
            {
                _transactionRepository.BeginTransaction();
                TourIssueReport tourIssueReport = _tourIssueReportRepository.Get(tourIssueReportDto.Id);
                bool success = tourIssueReport.ReadNotification(tourNotificationId);
                _tourIssueReportRepository.Update(tourIssueReport);
                _transactionRepository.CommitTransaction();
                if (success){
                return MapToDto(tourIssueReport);
                }
                else{
                return Result.Fail(FailureCode.NotFound).WithError("Tour notification doesn't exist");
                }
            }
            catch (Exception e)
            {
                _transactionRepository.RollbackTransaction();
                return Result.Fail(FailureCode.Conflict).WithError(e.Message);
            }
        }
    }
}
