using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourIssueCommentService : CrudService<TourIssueCommentDto, TourIssueComment>, ITourIssueCommentService
    {
        private readonly ICrudRepository<TourIssueReport> _tourIssueReportRepository;
        private readonly ICrudRepository<TourIssueNotification> _tourIssueNotificationRepository;
        private readonly ICrudRepository<Tour> _tourRepository;
        private readonly ICrudRepository<TourIssueComment> _tourIssueCommentRepository;
        private readonly ITransactionRepository _transactionRepository;
        public TourIssueCommentService(ICrudRepository<TourIssueReport> repository,ICrudRepository<TourIssueNotification> tourNotificationRepository, ICrudRepository<Tour> tourRepository, ICrudRepository<TourIssueComment> tourIssueCommentRepository, ITransactionRepository transactionRepository, IMapper mapper) : base(tourIssueCommentRepository, mapper)
        {
            _tourIssueReportRepository = repository;
            _tourIssueNotificationRepository = tourNotificationRepository;
            _tourRepository = tourRepository;
            _tourIssueCommentRepository = tourIssueCommentRepository;
            _transactionRepository = transactionRepository;
        }

        public Result<TourIssueCommentDto> CreateComment(TourIssueCommentDto tourIssueComment, int fromId)
        {
            try
            {
                _transactionRepository.BeginTransaction();
                TourIssueComment tourIssueReportComment = MapToDomain(tourIssueComment);
                TourIssueReport report = _tourIssueReportRepository.Get(tourIssueReportComment.Id);
                Tour tour = _tourRepository.Get(report.TourId);
                if (report.UserId == fromId)
                {
                    var notif = new TourIssueNotification(fromId, tour.UserId, BuildingBlocks.Core.Domain.Enums.TourIssueNotificationStatus.Unread, report.Id);
                    _tourIssueNotificationRepository.Create(notif);
                }
                else
                {
                    var notif = new TourIssueNotification(fromId, tourIssueReportComment.UserId, BuildingBlocks.Core.Domain.Enums.TourIssueNotificationStatus.Unread, report.Id);
                    _tourIssueNotificationRepository.Create(notif);
                }
                var results = _tourIssueCommentRepository.Create(tourIssueReportComment);

                _transactionRepository.CommitTransaction();
                return MapToDto(results);
            }
            catch (Exception e)
            {
                _transactionRepository.RollbackTransaction();
                return Result.Fail(FailureCode.Conflict).WithError(e.Message);
            }
        }

        public PagedResult<TourIssueCommentDto> GetPaged(long reportId, int page, int pageSize)
        {
            List<TourIssueCommentDto> tourIssueCommentDtos = new List<TourIssueCommentDto>();
            var allIssueComments = _tourIssueCommentRepository.GetPaged(1, int.MaxValue);
            
            var comments = allIssueComments.Results.Where(t => t.TourIssueReportId == reportId).ToList();
            foreach(var c in comments)
                tourIssueCommentDtos.Add(MapToDto(c));

            return new PagedResult<TourIssueCommentDto>(tourIssueCommentDtos, tourIssueCommentDtos.Count());
        }

    }
}
