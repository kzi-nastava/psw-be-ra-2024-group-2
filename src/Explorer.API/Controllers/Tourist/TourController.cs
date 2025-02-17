using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using FluentResults;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Explorer.API.Controllers;

[Route("api/tour")]
public class TourController : BaseApiController
{
    private readonly ITourService _tourService;

    public TourController(ITourService tourService)
    {
        _tourService = tourService;
    }


    // this is also used for updating tour
    [HttpPost]
    public ActionResult<TourCreateDto> CreateTour([FromBody] TourCreateDto tourCreate)
    {
        if (tourCreate.Id == null)
        {
            var createResult = _tourService.Create(tourCreate);
            return CreateResponse(createResult);
        }

        var updateResult = _tourService.Update(tourCreate);
        return CreateResponse(updateResult);
    }

    [HttpGet("test")]
    public string Test()
    {
        return "testing tour controller";
    }

    [HttpGet("getGuideTours/{guideId}")]
    public ActionResult<IEnumerable<Tour>> getGuideTours(long guideId)
    {
        var tours = _tourService.GetGuideTours(guideId);

        if (tours == null)
        {
            return NotFound();  // Return 404 if no tours are found
        }

        return Ok(tours);  // Return 200 OK with the tours
    }

    [HttpGet("getAllTours")]
    public ActionResult<IEnumerable<Tour>> getAllTours()
    {
        var tours = _tourService.GetAllTours();

        if (tours == null)
        {
            return NotFound();  // Return 404 if no tours are found
        }

        return Ok(tours);  // Return 200 OK with the tours
    }

    [HttpGet("getById/{tourId}")]
    public ActionResult<Tour> getTourById(long tourId)
    {
        var tour = _tourService.GetTourById(tourId);

        if (tour == null)
        {
            return NotFound();  // Return 404 if no tours are found
        }

        return Ok(tour);  // Return 200 OK with the tours
    }

    [HttpPost("cancelTour/{tourId}")]
    public ActionResult CancelTour(long tourId)
    {
        bool exists = _tourService.Exists(tourId);

        if (!exists)
        {
            return NotFound(); // Return 404 if the tour does not exist
        }

        bool cancelled = _tourService.IfCancelled(tourId);

        if (!cancelled)
        {
            var result = Result.Fail("Tour is already cancelled!");
            return CreateResponse(result); // Return 404 if the tour does not exist
        }

        //tour.Status = "Cancelled";
        _tourService.CancelTour(tourId); // Save changes

        return Ok("Tour has been cancelled successfully.");
    }

}