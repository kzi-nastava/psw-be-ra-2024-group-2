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
    public class ObjectService : CrudService<ObjectDto, TourObject>, IObjectService
    {
        private readonly ICrudRepository<TourObject> _objectRepository;
        public ObjectService(ICrudRepository<TourObject> repository, IMapper mapper) : base(repository, mapper)
        {
            _objectRepository = repository;
        }

        public Result<ObjectDto> CreateObject(ObjectDto tourObject)
        {
            TourObject tourObj = _objectRepository.Get(tourObject.Id);
            TourObject newObject = _objectRepository.Create(tourObj);
            return MapToDto(newObject);
        }
    }
}
