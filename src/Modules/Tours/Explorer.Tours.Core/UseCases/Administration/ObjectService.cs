using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
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
                var category = (ObjectCategory)Enum.Parse(typeof(ObjectCategory), dto.Category, true);
                var obj = new TourObject(dto.Name, dto.Description, category, dto.Longitude, dto.Latitude,
                    new Image(dto.Image.Data, dto.Image.UploadedAt, dto.Image.MimeType));

               var res =  _objectRepository.Create(obj);
                return MapToDto(res);
            }
            catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
            
        }
    }
}
