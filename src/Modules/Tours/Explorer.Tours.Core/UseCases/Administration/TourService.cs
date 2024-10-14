using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourService : CrudService<TourDto, Tour>, ITourService
    {
        private readonly ICrudRepository<Tour> _tourRepository;

        public TourService(ICrudRepository<Tour> tourRepository, IMapper mapper) : base(tourRepository, mapper)
        {
            _tourRepository = tourRepository;
        }

        public Result<TourDto> CreateTour(TourDto dto, int userId)
        {

            Validate(dto);
            var result = _tourRepository.Create(MapToDomain(dto));
            return MapToDto(result);
        }

        public PagedResult<TourDto> GetAllByUserId(int userId)
        {
            var allTours = _tourRepository.GetPaged(1, int.MaxValue);


            var filteredTours = allTours.Results
                               .Where(tour => tour.UserId == userId)
                               .Select(tour => MapToDto(tour))
                               .ToList();

            return new PagedResult<TourDto>(filteredTours, filteredTours.Count());
        }

        private void Validate(TourDto dto)
        {
           
            if (dto.Price != 0) throw new ArgumentException("Price must be 0");
            if (dto.Status != TourDto.TourStatus.Draft) throw new ArgumentException("Invalid Status");
        }

    }
}
