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
        private readonly IUserLevelRepository _userLevelRepository;

        public EncounterService(IEncounterRepository encounterRepository, IImageRepository imageRepository, IUserLevelRepository userLevelRepository)
        {
            _encounterRepository = encounterRepository;
            _imageRepository = imageRepository;
            _userLevelRepository = userLevelRepository;
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
                    Lattitude = encounterDto.Lattitude,
                    Longitude = encounterDto.Longitude,

                };

                _encounterRepository.AddEncounter(encounter);

                // Return success result
                return Result.Ok(new SocialEncounterDto
                {
                    Id = encounter.Id,
                    Name = encounter.Name,
                    Description = encounter.Description,
                    RangeInMeters = encounter.RangeInMeters,
                    RequiredPeople = encounter.RequiredPeople,
                    Lattitude = encounterDto.Lattitude,
                    Longitude = encounterDto.Longitude,
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
                    Lattitude = encounter.Lattitude,
                    Longitude = encounter.Longitude,
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
                    Lattitude = encounterDto.Lattitude,
                    Longitude = encounterDto.Longitude,
                };

                _encounterRepository.AddEncounter(encounter);

                return Result.Ok(new MiscEncounterDto
                {
                    Id = encounter.Id,
                    Name = encounter.Name,
                    Description = encounter.Description,
                    ActionDescription = encounter.ActionDescription,
                    Lattitude = encounterDto.Lattitude,
                    Longitude = encounterDto.Longitude,
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to create misc encounter: {ex.Message}");
            }
        }


        // Update encounter methods
        public Result<SocialEncounterDto> UpdateSocialEncounter(UnifiedEncounterDto encounterDto)
        {
            SocialEncounterDto socialEncounterDto = new SocialEncounterDto();
            if (encounterDto.SocialRangeInMeters != null)
            {
                socialEncounterDto.RangeInMeters = encounterDto.SocialRangeInMeters.Value;
                socialEncounterDto.RequiredPeople = encounterDto.RequiredPeople.Value;
                socialEncounterDto.Description = encounterDto.Description;
                socialEncounterDto.Name = encounterDto.Name;
                socialEncounterDto.TouristIds = encounterDto.TouristIds;
                socialEncounterDto.Lattitude = encounterDto.Latitude;
                socialEncounterDto.Longitude = encounterDto.Longitude;
            }
            var existingEncounter = (SocialEncounter)_encounterRepository.GetById(encounterDto.Id);
            if (existingEncounter == null)
            {
                return Result.Fail("Social encounter not found.");
            }
            
            existingEncounter.Name = socialEncounterDto.Name;
            existingEncounter.Description = socialEncounterDto.Description;
            existingEncounter.RangeInMeters = socialEncounterDto.RangeInMeters;
            existingEncounter.RequiredPeople = socialEncounterDto.RequiredPeople;
            existingEncounter.TouristIds = socialEncounterDto.TouristIds;
            existingEncounter.Lattitude = socialEncounterDto.Lattitude;
            existingEncounter.Longitude = socialEncounterDto.Longitude;

            if (existingEncounter.TouristIds.Count > 9)
            {
                foreach (var touristId in existingEncounter.TouristIds)
                {
                    var tourist = _userLevelRepository.GetById(touristId);
                    if (tourist == null)
                    {
                        _userLevelRepository.AddUserLevel(new UserLevel(touristId, 1,0));
                    }
                    if (tourist?.Level < 5)
                    {
                        _userLevelRepository.UpdateUserLevel(new UserLevel(touristId,tourist.Level,tourist.Xp+10));
                    }
                }
            }
            _encounterRepository.UpdateEncounter(existingEncounter);

            return Result.Ok(new SocialEncounterDto
            {
                Id = existingEncounter.Id,
                Name = existingEncounter.Name,
                Description = existingEncounter.Description,
                RangeInMeters = existingEncounter.RangeInMeters,
                RequiredPeople = existingEncounter.RequiredPeople,
                Lattitude = existingEncounter.Lattitude,
                Longitude = existingEncounter.Longitude,
            });
        }
        public Result<HiddenLocationEncounterDto> UpdateHiddenLocationEncounter(UnifiedEncounterDto encounterDto)
        {
            try
            {
                HiddenLocationEncounterDto hiddenLocationEncounterDto = new HiddenLocationEncounterDto();
                if (encounterDto.HiddenLocationRangeInMeters != null)
                {
                    hiddenLocationEncounterDto.RangeInMeters = encounterDto.HiddenLocationRangeInMeters.Value;
                    hiddenLocationEncounterDto.Description = encounterDto.Description;
                    hiddenLocationEncounterDto.Name = encounterDto.Name;
                    hiddenLocationEncounterDto.TouristIds = encounterDto.TouristIds;
                    hiddenLocationEncounterDto.Lattitude = encounterDto.Latitude;
                    hiddenLocationEncounterDto.Longitude = encounterDto.Longitude;
                    hiddenLocationEncounterDto.TargetLatitude = encounterDto.TargetLatitude.Value;
                    hiddenLocationEncounterDto.TargetLongitude = encounterDto.TargetLongitude.Value;
                }
                var existingEncounter = (HiddenLocationEncounter)_encounterRepository.GetById(encounterDto.Id);
                if (existingEncounter == null)
                {
                    return Result.Fail("Hidden location encounter not found.");
                }
                existingEncounter.Lattitude = hiddenLocationEncounterDto.Lattitude;
                existingEncounter.Longitude = hiddenLocationEncounterDto.Longitude;
                existingEncounter.Name = hiddenLocationEncounterDto.Name;
                existingEncounter.TouristIds = hiddenLocationEncounterDto.TouristIds;
                existingEncounter.Description = hiddenLocationEncounterDto.Description;
                existingEncounter.TargetLatitude = hiddenLocationEncounterDto.TargetLatitude;
                existingEncounter.TargetLongitude = hiddenLocationEncounterDto.TargetLongitude;
                existingEncounter.RangeInMeters = hiddenLocationEncounterDto.RangeInMeters;

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

        public Result<MiscEncounterDto> UpdateMiscEncounter(UnifiedEncounterDto encounterDto)
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
                existingEncounter.TouristIds = encounterDto.TouristIds;

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
                    Lattitude = encounter.Lattitude,
                    Longitude = encounter.Longitude,
                };

                return Result.Ok(encounterDto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to retrieve encounter: {ex.Message}");
            }
        }

        public Result<List<UnifiedEncounterDto>> GetAll()
        {
            try
            {
                var encounters = _encounterRepository.GetAll();
                var encounterDtos = new List<UnifiedEncounterDto>();

                foreach (var encounter in encounters)
                {
                    // Debugging: Log the type of the encounter to ensure it's being recognized correctly
                    string encounterType = encounter.GetType().Name; // This will return the type name as a string (e.g., "SocialEncounter", "HiddenLocationEncounter")

                    UnifiedEncounterDto encounterDto = encounter switch
                    {
                        SocialEncounter socialEncounter => new UnifiedEncounterDto
                        {
                            Id = socialEncounter.Id,
                            Name = socialEncounter.Name,
                            Description = socialEncounter.Description,
                            Latitude = socialEncounter.Lattitude,
                            Longitude = socialEncounter.Longitude,
                            EncounterType = "Social",
                            RequiredPeople = socialEncounter.RequiredPeople,
                            SocialRangeInMeters = socialEncounter.RangeInMeters,
                            TouristIds = socialEncounter.TouristIds
                        },
                        HiddenLocationEncounter hiddenLocationEncounter => new UnifiedEncounterDto
                        {
                            Id = hiddenLocationEncounter.Id,
                            Name = hiddenLocationEncounter.Name,
                            Description = hiddenLocationEncounter.Description,
                            Latitude = hiddenLocationEncounter.Lattitude,
                            Longitude = hiddenLocationEncounter.Longitude,
                            EncounterType = "HiddenLocation",
                            TargetLatitude = hiddenLocationEncounter.TargetLatitude,
                            TargetLongitude = hiddenLocationEncounter.TargetLongitude,
                            HiddenLocationRangeInMeters = hiddenLocationEncounter.RangeInMeters,
                            Image = new EncounterImageDto
                            {
                                Data = hiddenLocationEncounter.Image?.Data ?? string.Empty,
                                UploadedAt = hiddenLocationEncounter.Image?.UploadedAt ?? DateTime.MinValue,
                                MimeType = hiddenLocationEncounter.Image?.MimeType.ToString() ?? "default-mime-type"
                            }
                        },
                        MiscEncounter miscEncounter => new UnifiedEncounterDto
                        {
                            Id = miscEncounter.Id,
                            Name = miscEncounter.Name,
                            Description = miscEncounter.Description,
                            Latitude = miscEncounter.Lattitude,
                            Longitude = miscEncounter.Longitude,
                            EncounterType = "Misc",
                            ActionDescription = miscEncounter.ActionDescription
                        },
                        _ => throw new InvalidOperationException($"Unknown encounter type: {encounter.GetType().Name}")
                    };

                    encounterDtos.Add(encounterDto); // Add the derived DTO to the list
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
                    Lattitude = encounter.Lattitude,
                    Longitude = encounter.Longitude,
                };

                return Result.Ok(encounterDto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to retrieve encounter: {ex.Message}");
            }            
        }

        public Result<string> RemoveFromSocialEncounters(int id)
        {
            var list = _encounterRepository.GetAll();
            var socialEncounter = list.OfType<SocialEncounter>().Where(x => x.TouristIds.Contains(id));
            if (socialEncounter == null)
            {
                return Result.Fail("Social encounter not found.");
            }
            foreach (var encounter in socialEncounter)
            {
                encounter.TouristIds.Remove(id);
                _encounterRepository.UpdateEncounter(encounter);
            }
            return Result.Ok("Fixed the list ");
        }
    }
}
