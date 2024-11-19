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
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Public.Tourist;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class PersonalDairyService : CrudService<PersonalDairyDto, PersonalDairy>, IPersonalDairyService
    {
    private readonly ICrudRepository<Tour> _tourRepository;
    private readonly ICrudRepository<PersonalDairy> _personalDairyCrudRepository;
    private readonly ITourPurchaseTokenService_Internal _tourPurchaseTokenService;
    private readonly IPersonalDairyRepository _personalDairyRepository;
    private readonly IMapper _mapper;


        public PersonalDairyService(ICrudRepository<Tour> tourRepository, ICrudRepository<PersonalDairy> personalDairyCrudRepository, ITourPurchaseTokenService_Internal tourPurchaseTokenService, IPersonalDairyRepository personalDairyRepository, IMapper mapper) : base(personalDairyCrudRepository, mapper)
        {
            _tourRepository = tourRepository;
            _personalDairyCrudRepository = personalDairyCrudRepository;
            _tourPurchaseTokenService = tourPurchaseTokenService;
            _personalDairyRepository = personalDairyRepository;
            _mapper = mapper;
        }


        public Result<PersonalDairyDto> Create(PersonalDairyDto personalDairyDto)
        {
            try
            {
                var dairy = _mapper.Map<PersonalDairy>(personalDairyDto);

                var createdDairy = _personalDairyCrudRepository.Create(dairy);

           
                return Result.Ok(_mapper.Map<PersonalDairyDto>(createdDairy));
            }
            catch (Exception ex)
            {
                return Result.Fail<PersonalDairyDto>("An error occurred while creating the personal diary: " + ex.Message);
            }
        }


        public Result Delete(long id)
        {
            try
            {
                var existingDairy = _personalDairyCrudRepository.Get(id);
                if (existingDairy == null)
                {
                    return Result.Fail($"Personal dairy with ID {id} not found.");
                }

                _personalDairyCrudRepository.Delete(id);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while deleting the personal dairy: {ex.Message}");
            }
        }

        public Result<PersonalDairyDto> Update(long id, PersonalDairyDto updatedDairyDto)
        {
            try
            {
                var existingDairy = _personalDairyCrudRepository.Get(id);
                if (existingDairy == null)
                {
                    return Result.Fail<PersonalDairyDto>($"Personal dairy with ID {id} not found.");
                }

                var updatedDairy = _mapper.Map(updatedDairyDto, existingDairy);

                var result = _personalDairyCrudRepository.Update(updatedDairy);

                return Result.Ok(_mapper.Map<PersonalDairyDto>(result));
            }
            catch (Exception ex)
            {
                return Result.Fail<PersonalDairyDto>($"An error occurred while updating the personal dairy: {ex.Message}");
            }
        }
        public Result<IEnumerable<PersonalDairyDto>> GetAllForUser(long userId)
        {
            try
            {
                var dairies = _personalDairyRepository.GetAllCompletedByUserId(userId);

                var result = dairies.Select(d => _mapper.Map<PersonalDairyDto>(d));
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<PersonalDairyDto>>($"An error occurred while retrieving personal dairies: {ex.Message}");
            }
        }

    }
}
