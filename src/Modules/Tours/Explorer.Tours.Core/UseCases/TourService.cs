using System;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases
{
	public class TourService : ITourService
	{
        private readonly ITourRepository _tourRepository;

        public TourService(ITourRepository tourRepository)
		{
            _tourRepository = tourRepository;   
        }

        public bool Exists(long id)
        {
            return _tourRepository.Exists(id);
        }
        
        public bool IfCancelled(long id)
        {
            Tour? tour =  _tourRepository.GetTourByID(id);

            if(tour != null)
            {
                if(tour.Status == TourStatus.Cancelled)
                {
                    return true;
                }
            }
            return false;
        }

        public string CancelTour(long id)
        {
            string x = "da";

            Tour? tour = _tourRepository.GetTourByID(id);
  
            return x;
        }

        public Result<IEnumerable<TourCreateDto>> GetAllTours()
        {
            var tours = _tourRepository.GetAllTours();

            if (tours == null || !tours.Any())
            {
                return Result.Fail<IEnumerable<TourCreateDto>>("No tours found for this guide.");
            }

            var tourDtos = tours.Select(tour => new TourCreateDto
            {
                Name = tour.Name,
                Description = tour.Description,
                Difficulty = tour.Difficulty,
                Category = tour.Category.ToString(), // Convert the category enum to a string
                Price = tour.Price,
                Date = tour.Date,
                Id = tour.Id,
                Status = tour.Status.ToString(),
                // If tour.KeyPoints is not null, map it to a list of KeyPointDto objects
                KeyPoints = tour.KeyPoints != null
                        ? tour.KeyPoints.Select(kp => new KeyPointDto
                        {
                            Latitude = kp.Latitude,
                            Longitude = kp.Longitude,
                            Name = kp.Name,
                            Description = kp.Description
                        }).ToList()
                        : new List<KeyPointDto>() // Return an empty list if KeyPoints is null
            });


            // Return the mapped results as a success
            return Result.Ok(tourDtos);

        }

        public Result<IEnumerable<TourCreateDto>> GetGuideTours(long guideId)
        {
            // Retrieve the tours for the given guideId
            var tours = _tourRepository.GetGuideTours(guideId);

            // Check if there are any tours, and map them to TourCreateDto
            if (tours == null || !tours.Any())
            {
                return Result.Fail<IEnumerable<TourCreateDto>>("No tours found for this guide.");
            }

            // Map tours to TourCreateDto
            /*
            var tourDtos = tours.Select(tour => new TourCreateDto
            {
                Name = tour.Name,
                Description = tour.Description,
                Difficulty = tour.Difficulty,
                Category = tour.Category.ToString(),  // Assuming you want to return the category as a string
                Price = tour.Price,
                Date = tour.Date,
                Id = tour.Id
            }); */
            var tourDtos = tours.Select(tour => new TourCreateDto
            {
                Name = tour.Name,
                Description = tour.Description,
                Difficulty = tour.Difficulty,
                Category = tour.Category.ToString(), // Convert the category enum to a string
                Price = tour.Price,
                Date = tour.Date,
                Id = tour.Id,
                Status=tour.Status.ToString(),
                // If tour.KeyPoints is not null, map it to a list of KeyPointDto objects
                KeyPoints = tour.KeyPoints != null
                        ? tour.KeyPoints.Select(kp => new KeyPointDto
                        {
                            Latitude = kp.Latitude,
                            Longitude = kp.Longitude,
                            Name = kp.Name,
                            Description = kp.Description
                        }).ToList()
                        : new List<KeyPointDto>() // Return an empty list if KeyPoints is null
            });


            // Return the mapped results as a success
            return Result.Ok(tourDtos);
        }

        public Result<TourCreateDto> GetTourById(long tourId)
        {
            // Retrieve the tours for the given guideId
            var tour = _tourRepository.GetTourByID(tourId);

            // Check if there are any tours, and map them to TourCreateDto
            if (tour == null)
            {
                return Result.Fail<TourCreateDto>("No tour was found");
            }

            var tourDto = new TourCreateDto
            {
                Name = tour.Name,
                Description = tour.Description,
                Difficulty = tour.Difficulty,
                Category = tour.Category.ToString(), // Convert the category enum to a string
                Price = tour.Price,
                Date = tour.Date,
                Id = tour.Id,
                Status = tour.Status.ToString(),
                // If tour.KeyPoints is not null, map it to a list of KeyPointDto objects
                KeyPoints = tour.KeyPoints != null
                        ? tour.KeyPoints.Select(kp => new KeyPointDto
                        {
                            Latitude = kp.Latitude,
                            Longitude = kp.Longitude,
                            Name = kp.Name,
                            Description = kp.Description
                        }).ToList()
                        : new List<KeyPointDto>() // Return an empty list if KeyPoints is null
            };


            // Return the mapped results as a success
            return Result.Ok(tourDto);
        }

        public Result<TourCreateDto> Create(TourCreateDto tourDto)
        {
            if (!Enum.TryParse<TourCategory>(tourDto.Category, true, out var category))
            {
                // Handle invalid category (e.g., throw an exception or return an error)
                throw new ArgumentException($"Invalid category: {tourDto.Category}");
            }

            Tour newTour = new Tour(
                tourDto.Name,
                tourDto.Description,
                tourDto.Difficulty,
                category,   // category now is of type TourCategory
                tourDto.Price,
                tourDto.Date.ToUniversalTime(),
                tourDto.guideId
            );
            Tour tourResponse = _tourRepository.Create(newTour);

            List<KeyPoint> keyPoints = new List<KeyPoint>();

            if (tourDto.KeyPoints != null)
            {                
                foreach (var keyPointDto in tourDto.KeyPoints)
                {
                    KeyPoint newKeyPoint = new KeyPoint(keyPointDto.Latitude, keyPointDto.Longitude, keyPointDto.Name, keyPointDto.Description, tourResponse.Id);
                    keyPoints.Add(newKeyPoint);
                    Console.WriteLine("key point: " + keyPointDto.Name + ", tour id" + tourResponse.Id.ToString());
                    _tourRepository.CreateKeyPoint(newKeyPoint);
                }
            }


            return tourDto;
            //throw new NotImplementedException();
        }

        public Result Delete(long tourId)
        {
            _tourRepository.GetTourByID(tourId);
            throw new NotImplementedException();
        }

        public Result<TourCreateDto> Update(TourCreateDto tourDto)
        {
            // Fetch the existing tour by its ID
            
            long tourId = tourDto.Id.Value;
            if(tourId == null)
            {
                return Result.Fail<TourCreateDto>($"Tour ID is null");
            } 
            var existingTour = _tourRepository.GetTourByID(tourId);

            if (existingTour == null)
            {
                // Handle tour not found
                return Result.Fail<TourCreateDto>($"Tour with ID {tourId} not found");
            }

            // Ensure category is valid
            if (!Enum.TryParse<TourCategory>(tourDto.Category, true, out var category))
            {
                // Handle invalid category
                return Result.Fail<TourCreateDto>($"Invalid category: {tourDto.Category}");
            }

            Tour updatedTourFromDTO = new Tour(tourDto.Name, tourDto.Description, tourDto.Difficulty, category, tourDto.Price, tourDto.Date.ToUniversalTime(), tourDto.guideId);

            existingTour.UpdateTour(updatedTourFromDTO);

            // If you have KeyPoints that need to be updated

                    

            // Save the updated tour to the repository
            _tourRepository.Update(existingTour);



            var existingKeyPoints = existingTour.KeyPoints.ToList(); // Copy to avoid modifying collection while iterating
            var newKeyPoints = tourDto.KeyPoints ?? new List<KeyPointDto>();

            // Remove KeyPoints that are not in new list
            foreach (var keyPoint in existingKeyPoints)
            {
                if (!newKeyPoints.Any(kp => kp.Id == keyPoint.Id))
                {                    
                    _tourRepository.DeleteKeyPoint(keyPoint);
                }
            }

            // Update existing KeyPoints
            foreach (var keyPoint in newKeyPoints)
            {
                var existingKeyPoint = existingTour.KeyPoints.FirstOrDefault(kp => kp.Id == keyPoint.Id);
                if (existingKeyPoint != null)
                {
                    existingKeyPoint.UpdateKeyPoint(keyPoint.Latitude, keyPoint.Longitude, keyPoint.Name, keyPoint.Description);
                    _tourRepository.UpdateKeyPoint(existingKeyPoint);
                }
                else
                {
                    KeyPoint newKeyPoint = new KeyPoint(keyPoint.Latitude, keyPoint.Longitude, keyPoint.Name, keyPoint.Description, tourId);
                    _tourRepository.CreateKeyPoint(newKeyPoint);
                }
            }
            
            // Return the updated TourCreateDto as a success result
            return Result.Ok(tourDto);
        }

    }
}

