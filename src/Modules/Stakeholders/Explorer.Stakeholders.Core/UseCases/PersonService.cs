﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.Stakeholders.API.Public;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class PersonService : CrudService<PersonDto, Person>, IPersonService
    {
        private readonly ICrudRepository<Person> _personRepository;

        public PersonService(ICrudRepository<Person> personRepository, IMapper mapper) : base(personRepository, mapper)
        {
            _personRepository = personRepository;
        }

        public Result<PersonDto> UpdateTouristPosition(int userId, double latitude, double longitude)
        {
            try
            {
                var person = _personRepository.Get(userId);

                if (person == null)
                    return Result.Fail(FailureCode.NotFound).WithError("User not found");
                //tour.TouristPosition ??= new TouristPosition(latitude, longitude);
                TouristPosition tp = new TouristPosition();
                tp.UpdatePosition(latitude, longitude);
                person.TouristPosition = tp;
                _personRepository.Update(person);

                return MapToDto(person);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.Conflict).WithError(e.Message); //Conflict
            }
        }
    }
}
