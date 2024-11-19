using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.UseCases
{
    public class EncounterService : CrudService<EncounterDTO, Encounter>, IEncounterService
    {
        private readonly ICrudRepository<Encounter> repository;
        private readonly IMapper mapper;

        public EncounterService(ICrudRepository<Encounter> repository, IMapper mapper) : base(repository, mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public Result<EncounterDTO> Create(EncounterDTO encounter)
        {
            var result = repository.Create(MapToDomain(encounter));
            return Result.Ok(MapToDto(result));
        }

        public Result Delete(long id)
        {
            repository.Delete(id);
            return Result.Ok();
        }

        public Result<EncounterDTO> Get(long id)
        {
            return repository.Get(id) is Encounter encounter ? Result.Ok(MapToDto(encounter)) : Result.Fail<EncounterDTO>("Encounter not found");
        }
    }
}
