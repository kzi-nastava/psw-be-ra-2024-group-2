using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourReviewService : CrudService<TourReviewDto, TourReview>, ITourReviewService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ICrudRepository<TourReview> _reviewRepository;


        public TourReviewService(ICrudRepository<TourReview> repository, IMapper mapper, IImageRepository imageRepository, 
                                    ICrudRepository<TourReview> reviewRepository) : base(repository, mapper)
        {
            _imageRepository = imageRepository;
            _reviewRepository = reviewRepository;

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
            var filteredPagedResult = new PagedResult<TourReviewDto>(filteredReviews, pageSize);

            return Result.Ok(filteredPagedResult);
        }

        public override Result<TourReviewDto> Create(TourReviewDto dto)
        {


            TourReview review = new TourReview();

            review.Grade = dto.Grade;
            review.Comment = dto.Comment;
            review.UserId = dto.UserId;
            review.TourId = dto.TourId;
            review.ReviewDate = dto.ReviewDate;
            review.VisitDate = dto.VisitDate;

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
        
        public void ValidateDto(TourReviewDto tourReviewDto) 
        {
            
        }


    }
}
