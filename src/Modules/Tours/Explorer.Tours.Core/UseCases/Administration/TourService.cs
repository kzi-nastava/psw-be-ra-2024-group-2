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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourService : CrudService<TourDto, Tour>, ITourService
    {
        private readonly ICrudRepository<Tour> _tourRepository;

        public TourService(ICrudRepository<Tour> tourRepository,IMapper mapper) : base(tourRepository, mapper)
        {
            _tourRepository = tourRepository;
        }

        public PagedResult<TourDto> GetAllByUserId(int userId)
        {
            var allTours = _tourRepository.GetPaged(1, int.MaxValue);
            var filteredTours = allTours.Results.Where(tour => tour.UserId == userId);

            return (PagedResult<TourDto>)filteredTours;
        }
    }
}
