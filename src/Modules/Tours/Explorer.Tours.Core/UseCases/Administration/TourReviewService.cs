using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourReviewService : CrudService<TourReviewDto, TourReview>, ITourReviewService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ICrudRepository<TourReview> _reviewRepository;
        private readonly ICrudRepository<Tour> _tourRepository;
        private readonly ITourExecutionRepository _tourExecutionRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICrudRepository<TourPurchaseToken> _tourPurchaseTokenRepository;

        public TourReviewService(ICrudRepository<TourPurchaseToken> purchaseRepository, ICrudRepository<TourReview> repository, ITransactionRepository _transactionRepository, IMapper mapper, IImageRepository imageRepository, 
                                    ICrudRepository<TourReview> reviewRepository, ICrudRepository<Tour> tourRepository, ITourExecutionRepository tourExecutionRepository) : base(repository, mapper)
        {
            _imageRepository = imageRepository;
            _reviewRepository = reviewRepository;
            _tourRepository = tourRepository;
            _tourExecutionRepository = tourExecutionRepository;
            _tourPurchaseTokenRepository = purchaseRepository;
        }

        public Result<PagedResult<TourReviewDto>> GetPagedByTourId(int tourId, int page, int pageSize)
        {
            // Step 1: Fetch paged data from the base service
            var pagedResult = base.GetPaged(page, pageSize);

            if (pagedResult.IsFailed)
            {
                return Result.Fail(pagedResult.Errors);
            }

            // Step 2: Filter the results by TourId
            var filteredReviews = pagedResult.Value.Results
                .Where(r => r.TourId == tourId)
                .ToList();

            // Step 3: Create a new PagedResult based on filtered data
            var filteredPagedResult = new PagedResult<TourReviewDto>(filteredReviews, filteredReviews.Count);

            return Result.Ok(filteredPagedResult);
        }

        public Result<TourReviewDto> Update(TourReviewDto dto)
        {
            try
            {
                if (!IsPurchased(dto.TourId, dto.UserId))
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Nonexistant tour Id"); //400

                if (dto.Grade < 1 || dto.Grade > 5)
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Grade must be in range 1 - 5."); //400

                var allTourExecutions = _tourExecutionRepository.GetPaged(1, int.MaxValue);
                double currentProgress = 0;

                 foreach (var te in allTourExecutions.Results)
                 {
                     if (dto.UserId == te.UserId && dto.TourId == te.TourId)
                     {
                        currentProgress = te.GetProgress();
                         if ((te.GetProgress() < 0.35) || te.IsLastActivityOlderThanSevenDays())
                             return Result.Fail(FailureCode.InvalidArgument).WithError("You are not able to review this tour!");
                     }
                 }

                TourReview review = new TourReview(dto.Id);

                review.Grade = dto.Grade;
                review.Comment = dto.Comment;
                review.UserId = dto.UserId;
                review.TourId = dto.TourId;
                review.ReviewDate = DateTime.SpecifyKind(dto.ReviewDate, DateTimeKind.Utc); 
                review.VisitDate = DateTime.SpecifyKind(dto.VisitDate, DateTimeKind.Utc);  
                var newImage = new Image(
                       dto.Image.Data,
                       dto.Image.UploadedAt,
                       dto.Image.MimeType
                   );
                review.Image = newImage;
                review.Progress = currentProgress;
               
                TourReview newReview = _reviewRepository.Update(review);

                dto.Id = newReview.Id;
                dto.Grade = newReview.Grade;
                dto.Comment = newReview.Comment;
                dto.UserId = newReview.UserId;
                dto.TourId = newReview.TourId;
                dto.ReviewDate = DateTime.SpecifyKind(newReview.ReviewDate, DateTimeKind.Utc);
                dto.VisitDate = DateTime.SpecifyKind(newReview.VisitDate, DateTimeKind.Utc); 
                dto.Image = dto.Image;
                dto.Progress = currentProgress; // u trenutku pravljenja recenzije

                return dto;
            }
            catch(Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message); //404
            }
        }

        private bool IsPurchased(long tourId, long userId)
        {
            var tokens = _tourPurchaseTokenRepository.GetPaged(1, int.MaxValue);
            foreach (TourPurchaseToken token in tokens.Results)
            {
                if (token.TourId == tourId && token.UserId == userId)
                    return true;
            }
            return false;
        }
        public override Result<TourReviewDto> Create(TourReviewDto dto)
        {
            try
            {
                if(!IsPurchased(dto.TourId, dto.UserId))
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Nonexistant tour Id"); //400

                _tourRepository.Get(dto.TourId);
                double currentProgress = 0;

                if(dto.Grade < 1 || dto.Grade > 5)
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Nonexistant tour Id"); //400

                var allTourExecutions = _tourExecutionRepository.GetPaged(1, int.MaxValue);
                TourExecution tourExecution = null;

                foreach (var te in allTourExecutions.Results)
                {
                    if  (dto.TourId == te.TourId && dto.UserId == te.UserId )
                    {
                        dto.Progress = te.GetProgress();
                        currentProgress = te.GetProgress();

                        
                        tourExecution = te;
                    }
                }
                if (tourExecution == null)
                    return Result.Fail(FailureCode.InvalidArgument).WithError("You are not able to review this tour!");

                if ((tourExecution.GetProgress() < 0.35) || tourExecution.IsLastActivityOlderThanSevenDays()) // uslov
                    return Result.Fail(FailureCode.InvalidArgument).WithError("You are not able to review this tour!"); //exception

                TourReview review = new TourReview();

                review.Grade = dto.Grade;
                review.Comment = dto.Comment;
                review.UserId = dto.UserId;
                review.TourId = dto.TourId;
                review.ReviewDate = DateTime.SpecifyKind(dto.ReviewDate, DateTimeKind.Utc); // Ensure UTC
                review.VisitDate = DateTime.SpecifyKind(dto.VisitDate, DateTimeKind.Utc);   // Ensure UTC
                review.Progress = currentProgress; 
                if (dto.Image != null && !_imageRepository.Exists(dto.Image.Data))
                {
                    var newImage = new Image(
                        dto.Image.Data,
                        dto.Image.UploadedAt,
                        dto.Image.MimeType
                    );
                    review.Image = newImage;
                }
                else if (dto.Image != null && _imageRepository.Exists(dto.Image.Data))
                {
                    var image = _imageRepository.GetByData(dto.Image.Data);

                    review.ImageId = image.Id;
                    review.Image = image;
                }

                _reviewRepository.Create(review);

                return MapToDto(review);
            }
            catch(Exception ex) 
            {
                return Result.Fail(FailureCode.NotFound).WithError("Nonexistant tour Id"); //404
            }
        } 
        
        
    }
}
