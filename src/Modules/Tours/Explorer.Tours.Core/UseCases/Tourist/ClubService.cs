using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class ClubService : CrudService<ClubDto, Club>, IClubService
    {
        public ICrudRepository<Club> _clubRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public ClubService(ICrudRepository<Club> clubRepository, IImageRepository imageRepository, IUserRepository userRepository, ITransactionRepository transactionRepository, ITokenGenerator generator, IMapper mapper) : base(clubRepository, mapper)
        {
            _userRepository = userRepository;
            _clubRepository = clubRepository;
            _transactionRepository = transactionRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public Result<ClubDto> Create(int UserId, ClubDto clubDto)
        {
            if (string.IsNullOrWhiteSpace(clubDto.Name))
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("Name is required");
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
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Name is required");
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

                if (clubDto.ImageId.HasValue)
                {
                    var existingImage = _imageRepository.Get(clubDto.ImageId.Value);
                    if (existingImage != null)
                    {
                        club.ImageId = (int)existingImage.Id;
                    }

                    else
                    {
                        return Result.Fail(FailureCode.NotFound).WithError("Image not found");
                    }
                }

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
            var allClubs = _clubRepository.GetPaged(1, int.MaxValue);

            var filteredClubs = allClubs.Results
                .Select(club => MapToDto(club))
                .ToList();

            return new PagedResult<ClubDto>(filteredClubs, filteredClubs.Count());
        }
    }
}
