using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Internal;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class PersonalDairyService : BaseService<PersonalDairyDto, PersonalDairy>
    {
        private readonly IPersonalDairyRepository _personalDairyRepository;
        private readonly ICrudRepository<Tour> _tourRepository;
        private readonly ITourPurchaseTokenService_Internal _tourPurchaseTokenService;

        public PersonalDairyService( IMapper mapper, ICrudRepository<Tour> tourRepository, IPersonalDairyRepository personalDairyRepository, ITourPurchaseTokenService_Internal tourPurchaseTokenService) : base(mapper)
        {

            _tourRepository = tourRepository;
            _personalDairyRepository = personalDairyRepository;
            _tourPurchaseTokenService = tourPurchaseTokenService;
        }

        public Result<PersonalDairyDto> Create(int tourId, int userId, string title)
        {
            /*
             
            var tokens = _tourPurchaseTokenService.GetPaged(1, int.MaxValue).Value.Results
                .Where(t => t.UserId == userId && t.TourId == tourId)
                .ToList();

            if (tokens.Count == 0)
            {
                return Result.Fail(FailureCode.Conflict).WithError("Tour is not bought");
            }
             */
            PersonalDairy personalDairy = new PersonalDairy(userId, tourId, title );
            var result = _personalDairyRepository.Create(personalDairy);
            return MapToDto(result);
        }


    }
}
