using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class ClubService : CrudService<ClubDto, Club>, IClubService
    {
        public ClubService(ICrudRepository<Club> crudRepository, IMapper mapper) : base(crudRepository, mapper) { }
    }
}
