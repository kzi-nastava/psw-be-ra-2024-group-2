using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class ClubService : CrudService<ClubDto, Club>, IClubService
    {
        public ICrudRepository<Club> _clubRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public ClubService(ICrudRepository<Club> clubRepository, IImageRepository imageRepository, ITransactionRepository transactionRepository, IMapper mapper) : base(clubRepository, mapper)
        {
            _clubRepository = clubRepository;
            _transactionRepository = transactionRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public Result<ClubDto> Create(int UserId, ClubDto clubDto)
        {
            if (string.IsNullOrWhiteSpace(clubDto.Name))
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("TourName is required");
            }

            try
            {
                var club = _mapper.Map<Club>(clubDto);
                club.OwnerId = UserId;
                var result = _clubRepository.Create(club);
                return Result.Ok(_mapper.Map<ClubDto>(result));
            }
            catch (Exception ex)
            {
                return Result.Fail<ClubDto>("An error occurred while creating the club: " + ex.Message);
            }
        }


        public Result<ClubDto> Update(int id, ClubDto clubDto)
        {
            try
            {
                var club = _clubRepository.Get(id);

                if (club == null)
                {
                    return Result.Fail(FailureCode.NotFound).WithError("Club not found");
                }

                if (string.IsNullOrWhiteSpace(clubDto.Name))
                {
                    return Result.Fail(FailureCode.InvalidArgument).WithError("TourName is required");
                }

                var existingClub = _clubRepository.Get(id);
                if (existingClub != null && existingClub.Id != id)
                {
                    return Result.Fail(FailureCode.Conflict).WithError("Club name already exists");
                }

                _transactionRepository.BeginTransaction();

                club.Name = clubDto.Name;
                club.Description = clubDto.Description;
                club.OwnerId = clubDto.OwnerId;
                club.ImageId = clubDto.ImageId;

                var updatedClub = _clubRepository.Update(club);

                _transactionRepository.CommitTransaction();

                return MapToDto(updatedClub);
            }
            catch (KeyNotFoundException e)
            {
                if (_transactionRepository.HasActiveTransacation())
                    _transactionRepository.RollbackTransaction();

                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                if (_transactionRepository.HasActiveTransacation())
                    _transactionRepository.RollbackTransaction();

                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public PagedResult<ClubDto> GetAll()
        {
            var allClubs = _clubRepository.GetPaged(0, 0);

            var filteredClubs = allClubs.Results
                .Select(club => MapToDto(club))
                .ToList();

            return new PagedResult<ClubDto>(filteredClubs, filteredClubs.Count());
        }
    }
}
