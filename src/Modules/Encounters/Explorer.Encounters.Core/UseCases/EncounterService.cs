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
        private readonly IMapper _mapper;


        public EncounterService(IEncounterRepository encounterRepository, IImageRepository imageRepository, IUserLevelRepository userLevelRepository)
        {
            _encounterRepository = encounterRepository;
            _imageRepository = imageRepository;
            _userLevelRepository = userLevelRepository;
        }

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
                    TouristsInRange = new List<int>()
                };

                _encounterRepository.AddEncounter(encounter);

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
                    encounterDto.Lattitude,
                    encounterDto.Longitude,
                    encounterDto.RangeInMeters
                    ,encounterDto.IsForTour
                    ,encounterDto.TourId);

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
                    RangeInMeters = encounter.RangeInMeters,
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


        public Result<SocialEncounterDto> UpdateSocialEncounter(UnifiedEncounterDto encounterDto)
        {
            var existingEncounter = (SocialEncounter)_encounterRepository.GetById(encounterDto.Id);
            if (existingEncounter == null)
            {
                return Result.Fail("Social encounter not found.");
            }
                existingEncounter.RangeInMeters = encounterDto.SocialRangeInMeters.Value;
                existingEncounter.RequiredPeople = encounterDto.RequiredPeople.Value;
                existingEncounter.Description = encounterDto.Description;
                existingEncounter.Name = encounterDto.Name;
                existingEncounter.Lattitude = encounterDto.Latitude;
                existingEncounter.Longitude = encounterDto.Longitude;
                existingEncounter.TouristIds = encounterDto.TouristIds;
                existingEncounter.TouristsInRange = encounterDto.TouristsInRange;

            if (existingEncounter.TouristsInRange.Count == existingEncounter.RequiredPeople)
            {
                var userLevelsDict = _userLevelRepository.GetAllUserLevels().ToDictionary(t => t.UserId);

                foreach (var touristId in existingEncounter.TouristIds)
                {
                    if (!userLevelsDict.TryGetValue(touristId, out var tourist))
                    {
                        _userLevelRepository.AddUserLevel(new UserLevel(touristId, 1, 0));
                    }
                    else if (tourist.Level < 5)
                    {
                        var userLevel = userLevelsDict[touristId];
                        if (userLevel != null)
                        {
                            if (userLevel.Xp < 90)
                            {
                                userLevel.Xp += 10;
                                _userLevelRepository.UpdateUserLevel(new UserLevel(touristId, tourist.Level, tourist.Xp + 10));
                            }
                            else
                            {
                                userLevel.Level += 1;
                                userLevel.Xp = 0;
                                _userLevelRepository.UpdateUserLevel(new UserLevel(touristId, tourist.Level + 1, 0));
                            }
                        }
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
                TouristsInRange = existingEncounter.TouristsInRange
            });
        }

        public Result<List<int>> AddTouristToSocialEncounter(int touristId, long encounterId)
        {
            try
            {
                var encounter = _encounterRepository.GetById(encounterId) as SocialEncounter;
                if (encounter == null)
                {
                    return Result.Fail<List<int>>("Social encounter not found.");
                }

                var completedTouristIds = encounter.AddTouristInRange(touristId);

                _encounterRepository.UpdateEncounter(encounter);

                if (completedTouristIds != null)
                {
                    return Result.Ok(completedTouristIds);
                }

                // Challenge not completed yet
                return Result.Ok(new List<int>());
            }
            catch (Exception ex)
            {
                return Result.Fail<List<int>>($"Failed to add tourist to social encounter: {ex.Message}");
            }
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

        public Result<string> RemoveFromSocialEncounters(int touristId)
        {
            var encounters = _encounterRepository.GetAll();
            var socialEncounters = encounters.OfType<SocialEncounter>().Where(e => e.TouristsInRange.Contains(touristId));

            foreach (var encounter in socialEncounters)
            {
                encounter.TouristsInRange.Remove(touristId);
                _encounterRepository.UpdateEncounter(encounter);
            }

            return Result.Ok("Removed tourist from all relevant social encounters.");
        }


        public Result<string> UpdateTouristsInRange(long socialEncounterId, int touristId, bool isInRange)
        {
            try
            {
                var encounter = _encounterRepository.GetById(socialEncounterId) as SocialEncounter;

                if (encounter == null)
                {
                    return Result.Fail("Social encounter not found.");
                }

                if (isInRange)
                {
                    if (!encounter.TouristsInRange.Contains(touristId))
                    {
                        encounter.TouristsInRange.Add(touristId);
                    }
                }
                else
                {
                    encounter.TouristsInRange.Remove(touristId);
                }

                _encounterRepository.UpdateEncounter(encounter);
                return Result.Ok("Tourist list updated.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to update tourist list: {ex.Message}");
            }
        }
        public Result<string> MarkEncounterAsCompleted(long encounterId, int touristId)
        {
            try
            {
                var encounter = _encounterRepository.GetById(encounterId);

                if (encounter == null)
                {
                    return Result.Fail("Encounter not found.");
                }

                if (!encounter.TouristIds.Contains(touristId))
                {
                    encounter.TouristIds.Add(touristId);
                    _encounterRepository.UpdateEncounter(encounter);
                }

                return Result.Ok("Encounter marked as completed for the tourist.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to mark encounter as completed: {ex.Message}");
            }
        }

    }
}
