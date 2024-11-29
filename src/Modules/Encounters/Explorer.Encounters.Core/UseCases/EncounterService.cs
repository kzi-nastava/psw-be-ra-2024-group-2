using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
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

        private readonly IImageRepository _imageRepository;

        public EncounterService(IEncounterRepository encounterRepository, IImageRepository imageRepository)
        {
            _encounterRepository = encounterRepository;
            _imageRepository = imageRepository;
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

                };

                _encounterRepository.AddEncounter(encounter);

                // Return success result
                return Result.Ok(new SocialEncounterDto
                {
                    Id = encounter.Id,
                    Name = encounter.Name,
                    Description = encounter.Description,
                    RequiredPeople = encounter.RequiredPeople,
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
                Image image = new Image(encounterDto.Image.Data, encounterDto.Image.UploadedAt, encounterDto.Image.MimeType);
                var newImage = _imageRepository.Create(image);


                var encounter = new HiddenLocationEncounter(
                    encounterDto.Name,
                    encounterDto.Description,
                    newImage,
                    encounterDto.Lattitude,
                    encounterDto.Longitude,
                    encounterDto.Lattitude,     //encounterDto.TargetLatitude,
                    encounterDto.Longitude,     //encounterDto.TargetLongitude,
                    encounterDto.RangeInMeters);

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

            _encounterRepository.UpdateEncounter(existingEncounter);

            return Result.Ok(new SocialEncounterDto
            {
                Id = existingEncounter.Id,
                Name = existingEncounter.Name,
                Description = existingEncounter.Description,
                RangeInMeters = existingEncounter.RangeInMeters,
                RequiredPeople = existingEncounter.RequiredPeople,
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
