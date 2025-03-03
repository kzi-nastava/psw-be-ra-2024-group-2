using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using FluentResults;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.AspNetCore.Authorization;

using Explorer.Stakeholders.Core.UseCases;

namespace Explorer.API.Controllers;

[Route("api/tour")]
public class TourController : BaseApiController
{
    private readonly ITourService _tourService;
    //private readonly IUserService _userService;

    public TourController(ITourService tourService)
    {
        _tourService = tourService;
    }


    // this is also used for updating tour
    [HttpPost]
    [Authorize(Policy = "authorPolicy")]
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
    [Authorize(Policy = "authorPolicy")]
    public ActionResult<IEnumerable<TourCreateDto>> getGuideTours(long guideId)
    {
        var tours = _tourService.GetGuideTours(guideId);

        if (tours == null)
        {
            return NotFound();  // Return 404 if no tours are found
        }

        return Ok(tours);  // Return 200 OK with the tours
    }

    [HttpGet("getAllTours")]
    [Authorize(Policy = "touristPolicy")]
    public ActionResult<IEnumerable<TourCreateDto>> getAllTours()
    {
        var tours = _tourService.GetAllTours();
        Console.WriteLine("get all tours controller");

        if (tours == null)
        {
            return NotFound();  // Return 404 if no tours are found
        }
        //var userId = User.FindFirst("id")?.Value;
        //Console.WriteLine("current user id: " + userId.ToString());
        return Ok(tours);  // Return 200 OK with the tours
    }

    [HttpPost("buyTours")]
    [Authorize(Policy = "touristPolicy")]
    public ActionResult BuyTours([FromBody] List<long> tourIds)
    {
        Console.WriteLine("buy tours controller");
        Console.WriteLine($"User is buying tours: {string.Join(", ", tourIds)}");
        if (tourIds == null || tourIds.Count == 0)
        {
            return BadRequest("No tours selected for purchase.");
        }

        // Get current user ID from the token
        var userId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found.");
        }

        Console.WriteLine($"User {userId} is buying tours: {string.Join(", ", tourIds)}");

        var userIdLong = long.Parse(userId);

        var result = _tourService.BuyTours(userIdLong, tourIds);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        //_tourService.BuyTours(userIdLong, tourIds);

        return Ok("Tours purchased successfully.");
    }


    [HttpGet("getById/{tourId}")]
    [Authorize(Policy = "touristOrAuthorPolicy")]
    public ActionResult<Tour> getTourById(long tourId)
    {
        var tour = _tourService.GetTourById(tourId);

        if (tour == null)
        {
            return NotFound();  // Return 404 if no tours are found
        }

        return Ok(tour);  // Return 200 OK with the tours
    }

    /*
    [HttpGet("sendTestEmail")]
    [Authorize(Policy = "touristOrAuthorPolicy")]
    public IActionResult SendTestEmail()
    {
        try
        {
            _tourService.SendTestEmail();  // Call the method in EmailService to send the test email
            return Ok("Test email sent successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error sending test email: {ex.Message}");  // Return 500 if there's an error
        }
    }*/

    [HttpPost("createTourRate")]
    [Authorize(Policy = "touristPolicy")]
    public IActionResult CreateTourRate([FromBody] TourRateDto tourRateDto)
    {
        try
        {
            
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }
            var userIdLong = long.Parse(userId);

            var userUsername = User.FindFirst("Username")?.Value;
            Console.WriteLine("Tourist rate username: " + userUsername);


            tourRateDto.TouristId = userIdLong;
            tourRateDto.TouristUsername = userUsername;
            // Call the service to create the TourRate using the provided DTO
            var result = _tourService.CreateTourRate(tourRateDto);

            // Check if creation was successful
            if (result.IsSuccess)
            {
                return Ok($"Tour rate for TourId {tourRateDto.TourId} created successfully.");
            }

            // If the creation failed, return a failure response with the error message
            return BadRequest($"Error creating tour rate: {string.Join(", ", result.Errors)}");
        }
        catch (Exception ex)
        {
            // Return 500 if there is an exception
            return StatusCode(500, $"Error creating tour rate: {ex.Message}");
        }
    }

    [HttpGet("getTourRateByUser/{tourId}")]
    [Authorize(Policy = "touristPolicy")]
    public IActionResult GetTourRate(long tourId)
    {
        try
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }
            var userIdLong = long.Parse(userId);

            var result = _tourService.GetTourRateByUser(userIdLong, tourId);

            if (!result.IsSuccess)
            {
                return NotFound("Tour rate not found.");
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving tour rate: {ex.Message}");
        }
    }

    [HttpGet("getTourRates/{tourId}")]
    [Authorize(Policy = "authorPolicy")]
    public IActionResult GetTourRates(long tourId)
    {
        try
        {
            var guideId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(guideId))
            {
                return Unauthorized("User ID not found.");
            }
            var guideIdLong = long.Parse(guideId);

            // Check if the user is a tour guide for this tour
            bool isTourGuide = _tourService.CheckIfTourGuide(guideIdLong, tourId);
            if (!isTourGuide)
            {
                return Forbid("You are not authorized to view tour ratings.");
            }

            var result = _tourService.GetTourRates(tourId);

            if (!result.IsSuccess)
            {
                return StatusCode(500, "Error retrieving tour rates.");
            }

            return Ok(result.Value ?? new List<TourRateDto>()); // Return empty list if null
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving tour rates: {ex.Message}");
        }
    }




    [HttpPost("cancelTour/{tourId}")]
    [Authorize(Policy = "authorPolicy")]
    public ActionResult CancelTour(long tourId)
    {
        bool exists = _tourService.Exists(tourId);

        if (!exists)
        {
            return NotFound(); // Return 404 if the tour does not exist
        }

        bool cancelled = _tourService.IfCancelled(tourId);

        if (cancelled)
        {
            var result = Result.Fail("Tour is already cancelled!");
            return CreateResponse(result); // Return 404 if the tour does not exist
        }

        //tour.Status = "Cancelled";
        _tourService.CancelTour(tourId); // Save changes

        return Ok("Tour has been cancelled successfully.");
    }

}