using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class ObjectService : CrudService<ObjectDto, TourObject>, IObjectService
    {
        private readonly ICrudRepository<TourObject> _objectRepository;
        private readonly IImageRepository _imageRepository;
        public ObjectService(ICrudRepository<TourObject> repository, IImageRepository imageRepository, IMapper mapper) : base(repository, mapper)
        {
            _objectRepository = repository;
            _imageRepository = imageRepository;
        }

        public PagedResult<ObjectDto> GetAll()
        {
            var allObjects = _objectRepository.GetPaged(1, int.MaxValue);


            var objects = allObjects.Results
                               .Select(obj => MapToDto(obj))
                               .ToList();

            return new PagedResult<ObjectDto>(objects, objects.Count());
        }

        public Result<ObjectDto> UpdateObject(int id, double[] coordinates)
        {
            try
            {
                TourObject obj = _objectRepository.Get(id);
                obj.Longitude = coordinates[0];
                obj.Latitude = coordinates[1];
                _objectRepository.Update(obj);

                return MapToDto(obj);
            }
            catch (ArgumentException e)
            {

                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }

        }
        public Result<ObjectDto> Create(ObjectDto dto)
        {
            try
            {
                TourObject tourObject = MapToDomain(dto);

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
                    tourObject.ImageId = newImage.Id;
                    tourObject.Image = newImage;
                }
                else if (dto.Image != null && _imageRepository.Exists(dto.Image.Data))
                {
                    // If the image already exists, get the image from the repository
                    var image = _imageRepository.GetByData(dto.Image.Data);

                    // Update the person with the existing image
                    tourObject.ImageId = image.Id;
                    tourObject.Image = image;
                }
                // DODATO SAMO RADI TESTIRANJA BACKENDA PROMIJENICE SE INTEGRACIJOM MAPE

                _objectRepository.Create(tourObject);
                return MapToDto(tourObject);
            }
            catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
            
        }
    }
}
