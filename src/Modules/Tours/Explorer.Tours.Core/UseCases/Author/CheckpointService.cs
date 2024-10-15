using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Author
{
    public class CheckpointService : CrudService<CheckpointDto, Checkpoint>, ICheckpointService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ICrudRepository<Checkpoint> _checkpointRepository;

        public CheckpointService(ICrudRepository<Checkpoint> repository, IMapper mapper, IImageRepository imageRepository, ICrudRepository<Checkpoint> checkpointRepository) : base(repository, mapper) {
            _imageRepository = imageRepository;
            _checkpointRepository = checkpointRepository;
        }

        public override Result<CheckpointDto> Create(CheckpointDto dto)
        {
            try
            {
                /*_tourRepository.Get(dto.TourId);

                if (dto.Grade < 1 || dto.Grade > 5)
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Nonexistant tour Id"); //400 */

                Checkpoint checkpoint = new Checkpoint();

                checkpoint.Longitude = dto.Longitude;
                checkpoint.Latitude = dto.Latitude;
                checkpoint.Name = dto.Name;
                checkpoint.Description = dto.Description;
                checkpoint.Longitude = dto.Longitude;

                /*review.Comment = dto.Comment;
                review.UserId = dto.UserId;
                review.TourId = dto.TourId;
                review.ReviewDate = dto.ReviewDate;
                review.VisitDate = dto.VisitDate;*/

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
                    checkpoint.ImageId = newImage.Id;
                    checkpoint.Image = newImage;
                }
                else if (dto.Image != null && _imageRepository.Exists(dto.Image.Data))
                {
                    // If the image already exists, get the image from the repository
                    var image = _imageRepository.GetByData(dto.Image.Data);

                    // Update the person with the existing image
                    checkpoint.ImageId = image.Id;
                    checkpoint.Image = image;
                }

                _checkpointRepository.Create(checkpoint);


                // Return the result
                return MapToDto(checkpoint);
            }
            catch (Exception ex)
            {
                return Result.Fail(FailureCode.NotFound).WithError("Nonexistant tour Id"); //404
            }
        }
    }
}
