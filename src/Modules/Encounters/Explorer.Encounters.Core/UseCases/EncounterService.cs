using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Explorer.Encounters.Core.UseCases
{
    public class EncounterService : IEncounterService
    {
        private readonly IEncounterRepository _encounterRepository;
        public EncounterService(IEncounterRepository encounterRepository)
        {
            _encounterRepository = encounterRepository;
        }

        // Add encounter methods
        public Result<SocialEncounterDto> CreateSocialEncounter(SocialEncounterDto encounterDto)
        {
            try
            {
                var encounter = new SocialEncounter
                {
                    Name = encounterDto.Name,
                    Description = encounterDto.Description,
                    RangeInMeters = encounterDto.RangeInMeters,
                    RequiredPeople = encounterDto.RequiredPeople,
                    Latitude = encounterDto.Latitude,
                    Longitude = encounterDto.Longitude,
                };

                _encounterRepository.AddEncounter(encounter);

                // Return success result
                return Result.Ok(new SocialEncounterDto
                {
                    Id = encounter.Id,
                    Name = encounter.Name,
                    Description = encounter.Description,
                    RequiredPeople = encounter.RequiredPeople,
                    RangeInMeters = encounter.RangeInMeters,
                    Latitude = encounter.Latitude,
                    Longitude = encounter.Longitude,
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to create social encounter: {ex.Message}");
            }
        }

        public Result<HiddenLocationEncounterDto> CreateHiddenLocationEncounter(HiddenLocationEncounterDto encounterDto)
        {
            try
            {
                var encounter = new HiddenLocationEncounter
                {
                    Name = encounterDto.Name,
                    Description = encounterDto.Description,
                    TargetLongitude = encounterDto.TargetLongitude,
                    TargetLatitude = encounterDto.TargetLatitude,
                };

                _encounterRepository.AddEncounter(encounter);

                return Result.Ok(new HiddenLocationEncounterDto
                {
                    Id = encounter.Id,
                    Name = encounter.Name,
                    Description = encounter.Description,
                    TargetLongitude = encounter.TargetLongitude,
                    TargetLatitude = encounter.TargetLatitude,
                    // Map other properties
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to create hidden location encounter: {ex.Message}");
            }
        }

        public Result<MiscEncounterDto> CreateMiscEncounter(MiscEncounterDto encounterDto)
        {
            try
            {
                var encounter = new MiscEncounter
                {
                    Name = encounterDto.Name,
                    Description = encounterDto.Description,
                    ActionDescription = encounterDto.ActionDescription,
                };

                _encounterRepository.AddEncounter(encounter);

                return Result.Ok(new MiscEncounterDto
                {
                    Id = encounter.Id,
                    Name = encounter.Name,
                    Description = encounter.Description,
                    ActionDescription = encounter.ActionDescription,
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to create misc encounter: {ex.Message}");
            }
        }


        // Update encounter methods
        public Result<SocialEncounterDto> UpdateSocialEncounter(SocialEncounterDto encounterDto)
        {
            var existingEncounter = (SocialEncounter)_encounterRepository.GetById(encounterDto.Id);
            if (existingEncounter == null)
            {
                return Result.Fail("Social encounter not found.");
            }
            
            existingEncounter.Name = encounterDto.Name;
            existingEncounter.Description = encounterDto.Description;
            existingEncounter.RangeInMeters = encounterDto.RangeInMeters;
            existingEncounter.RequiredPeople = encounterDto.RequiredPeople;
            existingEncounter.Latitude = encounterDto.Latitude;
            existingEncounter.Longitude = encounterDto.Longitude;

            _encounterRepository.UpdateEncounter(existingEncounter);

            return Result.Ok(new SocialEncounterDto
            {
                Id = existingEncounter.Id,
                Name = existingEncounter.Name,
                Description = existingEncounter.Description,
                RangeInMeters = existingEncounter.RangeInMeters,
                RequiredPeople = existingEncounter.RequiredPeople,
                Longitude = existingEncounter.Longitude,
                Latitude = existingEncounter.Latitude,
            });

        }

        public Result<SocialEncounterDto> CheckTouristPosition(SocialEncounterDto encounterDto, double lon, double lat, int touristId)
        {
            try
            {
                if (encounterDto.IsActive == false)
                {
                    return Result.Fail($"Encounter is not active!");
                }
                var encounter = (SocialEncounter)_encounterRepository.GetById(encounterDto.Id);
                if(encounter == null)
                {
                    return Result.Fail("Social encounter not found.");
                }
                List<int> ids = new List<int>(encounter.TouristIds);

                if (encounter.CheckPoisition(lon, lat))
                {
                    if (!ids.Contains(touristId))
                    {
                        ids.Add(touristId);
                        encounter.UpdateTouristIds(ids);
                    }
                }
                else
                {
                    if (ids.Contains(touristId)) {
                        ids.Remove(touristId );
                        encounter.UpdateTouristIds(ids);
                    }
                }
                return Result.Ok(new SocialEncounterDto
                {
                    Id = encounter.Id,
                    Name = encounter.Name,
                    Description = encounter.Description,
                    RangeInMeters = encounter.RangeInMeters,
                    RequiredPeople = encounter.RequiredPeople,
                    Longitude = encounter.Longitude,
                    Latitude = encounter.Latitude,
                    TouristIds = encounter.TouristIds,
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to update social location encounter: {ex.Message}");
            }
        }

        public Result<HiddenLocationEncounterDto> UpdateHiddenLocationEncounter(HiddenLocationEncounterDto encounterDto)
        {
            try
            {
                var existingEncounter = (HiddenLocationEncounter)_encounterRepository.GetById(encounterDto.Id);
                if (existingEncounter == null)
                {
                    return Result.Fail("Hidden location encounter not found.");
                }

                existingEncounter.Name = encounterDto.Name;
                existingEncounter.Description = encounterDto.Description;
                existingEncounter.TargetLatitude = encounterDto.TargetLatitude;
                existingEncounter.TargetLongitude = encounterDto.TargetLongitude;

                _encounterRepository.UpdateEncounter(existingEncounter);

                return Result.Ok(new HiddenLocationEncounterDto
                {
                    Id = existingEncounter.Id,
                    Name = existingEncounter.Name,
                    Description = existingEncounter.Description,
                    TargetLongitude = existingEncounter.TargetLongitude,
                    TargetLatitude = existingEncounter.TargetLatitude,
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to update hidden location encounter: {ex.Message}");
            }
        }

        public Result<MiscEncounterDto> UpdateMiscEncounter(MiscEncounterDto encounterDto)
        {
            try
            {
                var existingEncounter = (MiscEncounter)_encounterRepository.GetById(encounterDto.Id);
                if (existingEncounter == null)
                {
                    return Result.Fail("Misc encounter not found.");
                }

                existingEncounter.Name = encounterDto.Name;
                existingEncounter.Description = encounterDto.Description;
                existingEncounter.ActionDescription = encounterDto.ActionDescription;

                _encounterRepository.UpdateEncounter(existingEncounter);

                return Result.Ok(new MiscEncounterDto
                {
                    Id = existingEncounter.Id,
                    Name = existingEncounter.Name,
                    Description = existingEncounter.Description,
                    // Map other properties
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to update misc encounter: {ex.Message}");
            }
        }

        // Retrieve encounter methods
        public Result<EncounterDto> GetByName(string name)
        {
            try
            {
                var encounter = _encounterRepository.GetByName(name);
                if (encounter == null)
                {
                    return Result.Fail("Encounter not found.");
                }

                var encounterDto = new EncounterDto
                {
                    Id = encounter.Id,
                    Name = encounter.Name,
                    Description = encounter.Description,
                    // Map other properties
                };

                return Result.Ok(encounterDto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to retrieve encounter: {ex.Message}");
            }
        }

        public Result<List<EncounterDto>> GetAll()
        {
            try
            {
                var encounters = _encounterRepository.GetAll();
                var encounterDtos = new List<EncounterDto>();

                foreach (var encounter in encounters)
                {
                    encounterDtos.Add(new EncounterDto
                    {
                        Id = encounter.Id,
                        Name = encounter.Name,
                        Description = encounter.Description,
                        // Map other properties
                    });
                }

                return Result.Ok(encounterDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to retrieve all encounters: {ex.Message}");
            }
        }

        // Delete encounter method
        public Result Delete(long id)
        {
            try
            {
                _encounterRepository.Delete(id);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to delete encounter: {ex.Message}");
            }
        }

        
        public Result<EncounterDto> GetById(long id)
        {           
            try
            {
                var encounter = _encounterRepository.GetById(id);
                if (encounter == null)
                {
                    return Result.Fail("Encounter not found.");
                }

                var encounterDto = new EncounterDto
                {
                    Id = encounter.Id,
                    Name = encounter.Name,
                    Description = encounter.Description,
                    // Map other properties
                };

                return Result.Ok(encounterDto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to retrieve encounter: {ex.Message}");
            }            
        }




    }
}
