using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourReviewService : CrudService<TourReviewDto, TourReview>, ITourReviewService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ICrudRepository<TourReview> _reviewRepository;
        private readonly ICrudRepository<Tour> _tourRepository;
        private readonly ICrudRepository<TourExecution> _tourExecutionRepository;

        public TourReviewService(ICrudRepository<TourReview> repository, IMapper mapper, IImageRepository imageRepository, 
                                    ICrudRepository<TourReview> reviewRepository, ICrudRepository<Tour> tourRepository, ICrudRepository<TourExecution> tourExecutionRepository) : base(repository, mapper)
        {
            _imageRepository = imageRepository;
            _reviewRepository = reviewRepository;
            _tourRepository = tourRepository;
            _tourExecutionRepository = tourExecutionRepository;
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
                // KOD ISPOD JE ZAKOMENTARISAN IZ RAZLOGA STO JOS UVIJEK NISU IMPLEMENTIRANE OSTALE ZAVISNE FUNKCIONALNOSTI
                // NE BRISATI!!!
                // NAKON STO KOLEGE IMPLEMENTIRAJU OSTATAK SVOJIH ZAHTJEVA, KOD ISPOD CE SE USPJESNO INTEGRISATI



                /* var allTourExecutions = _tourExecutionRepository.GetPaged(1, int.MaxValue);
                 foreach (var te in allTourExecutions.Results)
                 {
                     if (dto.UserId == te.UserId && dto.TourId == te.TourId)
                     {
                         if ((te.GetProgress() < 0.35) || te.IsLastActivityOlderThanSevenDays())
                             return Result.Fail(FailureCode.InvalidArgument).WithError("You are not able to review this tour!");
                     }
                 }*/

               // TourReview review = MapToDomain(dto);
               
                

                TourReview review = new TourReview(dto.Id);

                review.Grade = dto.Grade;
                review.Comment = dto.Comment;
                review.UserId = dto.UserId;
                review.TourId = dto.TourId;
                review.ReviewDate = DateTime.SpecifyKind(dto.ReviewDate, DateTimeKind.Utc); // Ensure UTC
                review.VisitDate = DateTime.SpecifyKind(dto.VisitDate, DateTimeKind.Utc);   // Ensure UTC
                var newImage = new Image(
                       dto.Image.Data,
                       dto.Image.UploadedAt,
                       dto.Image.MimeType
                   );
                review.Image = newImage;
               
                TourReview newReview = _reviewRepository.Update(review);

                dto.Id = newReview.Id;
                dto.Grade = newReview.Grade;
                dto.Comment = newReview.Comment;
                dto.UserId = newReview.UserId;
                dto.TourId = newReview.TourId;
                dto.ReviewDate = DateTime.SpecifyKind(newReview.ReviewDate, DateTimeKind.Utc); // Ensure UTC
                dto.VisitDate = DateTime.SpecifyKind(newReview.VisitDate, DateTimeKind.Utc);  // Ensure UTC
                dto.Image = dto.Image;



                return dto;
            }
            catch(Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message); //404
            }
        }
        public override Result<TourReviewDto> Create(TourReviewDto dto)
        {
            try
            {
                _tourRepository.Get(dto.TourId);
                
                if(dto.Grade < 1 || dto.Grade > 5)
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Nonexistant tour Id"); //400


                // KOD ISPOD JE ZAKOMENTARISAN IZ RAZLOGA STO JOS UVIJEK NISU IMPLEMENTIRANE OSTALE ZAVISNE FUNKCIONALNOSTI
                // NE BRISATI!!!
                // NAKON STO KOLEGE IMPLEMENTIRAJU OSTATAK SVOJIH ZAHTJEVA, KOD ISPOD CE SE USPJESNO INTEGRISATI



               /* var allTourExecutions = _tourExecutionRepository.GetPaged(1, int.MaxValue);
                foreach (var te in allTourExecutions.Results)
                {
                    if (dto.UserId == te.UserId && dto.TourId == te.TourId)
                    {
                        if ((te.GetProgress() < 0.35) || te.IsLastActivityOlderThanSevenDays())
                            return Result.Fail(FailureCode.InvalidArgument).WithError("You are not able to review this tour!");
                    }
                }*/

                TourReview review = new TourReview();

                review.Grade = dto.Grade;
                review.Comment = dto.Comment;
                review.UserId = dto.UserId;
                review.TourId = dto.TourId;
                review.ReviewDate = DateTime.SpecifyKind(dto.ReviewDate, DateTimeKind.Utc); // Ensure UTC
                review.VisitDate = DateTime.SpecifyKind(dto.VisitDate, DateTimeKind.Utc);   // Ensure UTC

                // Create the image and save it
                if (dto.Image != null && !_imageRepository.Exists(dto.Image.Data))
                {
                    // If the profile has an image, create a new image object with the data from the profile
                    var newImage = new Image(
                        dto.Image.Data,
                        dto.Image.UploadedAt,
                        dto.Image.MimeType
                    );

                    // Save the new image to the repository
                    _imageRepository.Create(newImage);

                    // Update the person with the new image
                    review.ImageId = newImage.Id;
                    review.Image = newImage;
                }
                else if (dto.Image != null && _imageRepository.Exists(dto.Image.Data))
                {
                    // If the image already exists, get the image from the repository
                    var image = _imageRepository.GetByData(dto.Image.Data);

                    // Update the person with the existing image
                    review.ImageId = image.Id;
                    review.Image = image;
                }

                _reviewRepository.Create(review);


                // Return the result
                return MapToDto(review);
            }
            catch(Exception ex) 
            {
                return Result.Fail(FailureCode.NotFound).WithError("Nonexistant tour Id"); //404
            }
        } 
        
        
    }
}
