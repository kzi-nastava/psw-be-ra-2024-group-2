using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
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
        private readonly ITransactionRepository _transactionRepository;
        public TourIssueReportService(ICrudRepository<TourIssueReport> repository, ICrudRepository<Tour> tourRepository, ICrudRepository<TourIssueComment> tourIssueCommentRepository, ITransactionRepository transactionRepository, IMapper mapper) : base(repository, mapper)
        {
            _tourIssueReportRepository = repository;
            _tourRepository = tourRepository;
            _tourIssueCommentRepository = tourIssueCommentRepository;
            _transactionRepository = transactionRepository;
        }

        public Result<TourIssueReportDto> Create(long userId, long tourId, TourIssueReportDto tourIssueReport)
        {
            try
            {
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

                return MapToDto(results);
            }
            catch(Exception e) 
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        
    }
}
