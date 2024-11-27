using Explorer.API.Controllers;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = "touristPolicy")]
[Route("api/user/personal-dairy")]
public class PersonalDairyController : BaseApiController
{
    private readonly IPersonalDairyService _personalDairyService;

    public PersonalDairyController(IPersonalDairyService personalDairyService)
    {
        _personalDairyService = personalDairyService;
    }

    [HttpGet("{userId}")]
    public ActionResult<IEnumerable<PersonalDairyDto>> GetAllForUser(long userId)
    {
        var result = _personalDairyService.GetAllForUser(userId);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }
    
    [HttpGet("TourExecutionId/{tourExecutionId}")]
    public ActionResult<PersonalDairyDto> GetByTourExecutionId(int tourExecutionId)
    {
        var diary = _personalDairyService.GetByTourExecutionId(tourExecutionId);

        if (diary.IsFailed || diary.Value == null)
        {
            return BadRequest(diary.Errors);
        }
        return Ok(diary.Value);

    }

    [HttpPost]
    public ActionResult<PersonalDairyDto> Create([FromBody] PersonalDairyDto dto)
    {
        var result = _personalDairyService.Create(dto);
        return CreateResponse(result);
    }

    [HttpPut("{id}")]
    public ActionResult<PersonalDairyDto> Update(long id, [FromBody] PersonalDairyDto dto)
    {
        var result = _personalDairyService.Update(id, dto);
        return CreateResponse(result);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(long id)
    {
        var result = _personalDairyService.Delete(id);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Personal dairy deleted successfully.");
    }

    // Dodavanje podrške za Chapters

    [HttpGet("{diaryId}/chapters")]
    public ActionResult<IEnumerable<ChapterDto>> GetAllChapters(long diaryId)
    {
        var result = _personalDairyService.GetAllChapters(diaryId);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost("{dairyId}/chapters")]
    public ActionResult AddChapter(long dairyId, ChapterDto chapterDto)
    {
        var result = _personalDairyService.AddChapterToDairy(dairyId, chapterDto);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return CreateResponse(result);
    }

    [HttpPut("{diaryId}/chapters/{chapterId}")]
    public ActionResult<ChapterDto> UpdateChapter(long diaryId, long chapterId, [FromBody] ChapterDto chapterDto)
    {
        var result = _personalDairyService.UpdateChapterInDairy(diaryId, chapterId, chapterDto);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return CreateResponse(result);
    }

    [HttpDelete("{diaryId}/chapters/{chapterId}")]
    public ActionResult DeleteChapter(long diaryId, long chapterId)
    {
        var result = _personalDairyService.RemoveChapterFromDairy(diaryId, chapterId);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok($"Chapter with ID {chapterId} deleted successfully.");
    }
}
