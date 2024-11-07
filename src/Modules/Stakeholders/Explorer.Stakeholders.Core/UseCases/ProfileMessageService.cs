using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
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

        public ProfileMessageService(ICrudRepository<ProfileMessage> repository, IMapper mapper) :
            base(repository, mapper)
        {
            _profileMessageRepository = repository;
        }
    }
}
