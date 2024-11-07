using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class ProfileMessageService : CrudService<ProfileMessageDto, ProfileMessage>, IProfileMessageService
    {
        private readonly ICrudRepository<ProfileMessage> _profileMessageRepository;
        private readonly ITransactionRepository _transactionRepository;

        public ProfileMessageService(ICrudRepository<ProfileMessage> repository, ITransactionRepository transactionRepository, IMapper mapper) :
            base(repository, mapper)
        {
            _profileMessageRepository = repository;
            _transactionRepository = transactionRepository;
        }

        public Result<ProfileMessageDto> Create(ProfileMessageDto profileMessageDto, long userId)
        {
            try
            {
                _transactionRepository.BeginTransaction();
                var profileMessage = new ProfileMessage
                {
                    SenderId = userId,
                    RecipientId = profileMessageDto.RecipientId,
                    Text = profileMessageDto.Text,
                    Resource = profileMessageDto.Resource,
                    SentAt = DateTime.UtcNow
                };
                var result = _profileMessageRepository.Create(profileMessage);

                _transactionRepository.CommitTransaction();

                return Result.Ok(MapToDto(result));
            }
            catch (Exception e)
            {

                _transactionRepository.RollbackTransaction();
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
    }
}
