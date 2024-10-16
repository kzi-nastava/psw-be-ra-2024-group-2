using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
                if (dto.Latitude < -90 || dto.Latitude > 90)
                {
                    throw new ArgumentException("Latitude must be between -90 and 90 degrees.");
                }

                if (dto.Longitude < -180 || dto.Longitude > 180)
                {
                    throw new ArgumentException("Longitude must be between -180 and 180 degrees.");
                }

                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    throw new ArgumentException("Name cannot be null or empty.");
                }

                if (dto.Description == null)
                {
                    throw new ArgumentException("Description cannot be null.");
                }

                Checkpoint checkpoint = new Checkpoint();

                checkpoint.Longitude = dto.Longitude;
                checkpoint.Latitude = dto.Latitude;
                checkpoint.Name = dto.Name;
                checkpoint.Description = dto.Description;
                checkpoint.Longitude = dto.Longitude;

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

                /*if(_checkpointRepository.Create(checkpoint) == null)
                {
                    return Result.Fail(FailureCode.NotFound).WithError("Nonexistant checkpoint Id"); //404
                }*/
                _checkpointRepository.Create(checkpoint);

                // Return the result
                return MapToDto(checkpoint);
            }
            catch (Exception ex)
            {
                return Result.Fail(FailureCode.Conflict).WithError("Checkpoint data non valid");

            }
        }
    }
}
