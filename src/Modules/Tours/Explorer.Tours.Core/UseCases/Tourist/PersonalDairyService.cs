using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Internal;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Public.Tourist;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class PersonalDairyService : CrudService<PersonalDairyDto, PersonalDairy>, IPersonalDairyService
    {
    private readonly ICrudRepository<Tour> _tourRepository;
    private readonly ICrudRepository<PersonalDairy> _personalDairyCrudRepository;
    private readonly ITourPurchaseTokenService_Internal _tourPurchaseTokenService;
    private readonly IPersonalDairyRepository _personalDairyRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;


        public PersonalDairyService(ICrudRepository<Tour> tourRepository, ICrudRepository<PersonalDairy> personalDairyCrudRepository, IImageRepository imageRepository, ITourPurchaseTokenService_Internal tourPurchaseTokenService, IPersonalDairyRepository personalDairyRepository, IMapper mapper) : base(personalDairyCrudRepository, mapper)
        {
            _tourRepository = tourRepository;
            _personalDairyCrudRepository = personalDairyCrudRepository;
            _imageRepository = imageRepository;
            _tourPurchaseTokenService = tourPurchaseTokenService;
            _personalDairyRepository = personalDairyRepository;
            _mapper = mapper;
        }


        public Result<PersonalDairyDto> Create(PersonalDairyDto personalDairyDto)
        {
            try
            {
                var dairy = _mapper.Map<PersonalDairy>(personalDairyDto);

                var createdDairy = _personalDairyCrudRepository.Create(dairy);
                createdDairy.ClosedAt = null;
                return Result.Ok(_mapper.Map<PersonalDairyDto>(createdDairy));
            }
            catch (Exception ex)
            {
                return Result.Fail<PersonalDairyDto>("An error occurred while creating the personal diary: " + ex.Message);
            }
        }


        public Result Delete(long id)
        {
            try
            {
                var existingDairy = _personalDairyCrudRepository.Get(id);
                if (existingDairy == null)
                {
                    return Result.Fail($"Personal dairy with ID {id} not found.");
                }

                _personalDairyCrudRepository.Delete(id);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while deleting the personal dairy: {ex.Message}");
            }
        }

        public Result<PersonalDairyDto> Update(long id, PersonalDairyDto updatedDairyDto)
        {
            try
            {
                var existingDairy = _personalDairyCrudRepository.Get(id);
                if (existingDairy == null)
                {
                    return Result.Fail<PersonalDairyDto>($"Personal dairy with ID {id} not found.");
                }

                var updatedDairy = _mapper.Map(updatedDairyDto, existingDairy);

                var result = _personalDairyCrudRepository.Update(updatedDairy);

                return Result.Ok(_mapper.Map<PersonalDairyDto>(result));
            }
            catch (Exception ex)
            {
                return Result.Fail<PersonalDairyDto>($"An error occurred while updating the personal dairy: {ex.Message}");
            }
        }
        public Result<IEnumerable<PersonalDairyDto>> GetAllForUser(long userId)
        {
            try
            {
                var dairies = _personalDairyRepository.GetAllByUserId(userId);

                var result = dairies.Select(d => _mapper.Map<PersonalDairyDto>(d));
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<PersonalDairyDto>>($"An error occurred while retrieving personal dairies: {ex.Message}");
            }
        }

        public Result<PersonalDairyDto> GetByTourExecutionId(int tourExecutionId)
        {
            var diary = _personalDairyRepository.GetByTourExecutionId(tourExecutionId);
            if (diary == null)
            {
                return Result.Fail(new Error("Diary not found"));
            }
            return MapToDto(diary);
        }


        public Result<IEnumerable<ChapterDto>> GetAllChapters(long dairyId)
        {
            var chapters = _personalDairyRepository.GetById(dairyId)?.Chapters;

            if (chapters == null)
                return Result.Fail<IEnumerable<ChapterDto>>($"Diary with ID {dairyId} not found.");

            var chapterDtos = chapters.Select(chapter => _mapper.Map<ChapterDto>(chapter));

            return Result.Ok(chapterDtos);
        }

        public Result AddChapterToDairy(long personalDairyId, ChapterDto chapterDto)
        {
            try
            {
                // Pronađi dnevnik prema ID-u
                var dairy = _personalDairyRepository.GetById(personalDairyId);
                if (dairy == null)
                    return Result.Fail($"Personal dairy with ID {personalDairyId} not found.");

                // Mapiraj ChapterDto u Chapter
                var newChapter = _mapper.Map<Chapter>(chapterDto);

                // Ako je slika poslata, obraditi je i povezati sa chapter-om
                if (chapterDto.Image != null)
                {
                    var image = _mapper.Map<Image>(chapterDto.Image);
                    _imageRepository.Create(image); // Sačuvaj sliku u bazi
                    //newChapter.AddImage(image); // Poveži sliku sa poglavljem
                }

                // Dodaj novi chapter u dnevnik
                dairy.AddChapter(newChapter.Title, newChapter.Text, newChapter.Image);

                // Ažuriraj dnevnik u bazi
                _personalDairyCrudRepository.Update(dairy);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while adding chapter: {ex.Message}");
            }
        }






        public Result RemoveChapterFromDairy(long personalDairyId, long chapterId)
        {
            try
            {
                var dairy = _personalDairyRepository.GetById(personalDairyId);
                if (dairy == null)
                    return Result.Fail($"Personal dairy with ID {personalDairyId} not found.");

                dairy.RemoveChapter(chapterId);
                _personalDairyCrudRepository.Update(dairy);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while removing chapter: {ex.Message}");
            }
        }

        public Result<ChapterDto> UpdateChapterInDairy(long personalDairyId, long chapterId, ChapterDto chapterDto)
        {
            try
            {
                var dairy = _personalDairyRepository.GetById(personalDairyId);
                if (dairy == null)
                    return Result.Fail<ChapterDto>($"Personal dairy with ID {personalDairyId} not found.");

                var existingChapter = dairy.Chapters.FirstOrDefault(c => c.ChapterId == chapterId);
                if (existingChapter == null)
                    return Result.Fail<ChapterDto>($"Chapter with ID {chapterId} not found in dairy ID {personalDairyId}.");

                existingChapter.UpdateTextAndTitle(chapterDto.Text, chapterDto.Title);

                _personalDairyCrudRepository.Update(dairy);

                var updatedChapterDto = _mapper.Map<ChapterDto>(existingChapter);
                return Result.Ok(updatedChapterDto);
            }
            catch (Exception ex)
            {
                return Result.Fail<ChapterDto>($"An error occurred while updating chapter: {ex.Message}");
            }
        }

        public bool IsEnableToChange(PersonalDairyDto personalDairyDto)
        {
            var diary = _mapper.Map<PersonalDairy>(personalDairyDto);
            if (diary.ClosedAt.HasValue)
            {
                if (diary.ClosedAt.Value.AddDays(3) < DateTime.Now)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }

    }
}
