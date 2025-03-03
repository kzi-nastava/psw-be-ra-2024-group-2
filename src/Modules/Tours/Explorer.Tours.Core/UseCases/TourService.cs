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
        //private readonly IUserRepository userRepository;

        public TourService(ITourRepository tourRepository)
		{
            _tourRepository = tourRepository;
        }

        public List<long> GetAllGuidesWithTourInLastMonth()
        {
            return _tourRepository.GetAllGuidesWithTourInLastMonth();
        }

        public List<long> GetAllGuides()
        {
            return _tourRepository.GetAllGuides();
        }

        /*
        public void SendTourReminderEmails()
        {
            var reminderTours = _tourRepository.GetReminderTours(); // Fetch tours happening in the next 48 hours

            foreach (var tour in reminderTours)
            {
                var purchaseEmails = _tourRepository.GetTourReminderEmails(tour.Id); // Get emails for this tour

                foreach (var email in purchaseEmails)
                {
                    var emailBody = GenerateTourReminderEmailBody(tour); // Generate email content
                    _emailService.SendEmail(email, "Reminder: Upcoming Tour", emailBody);
                }
            }
        }

        public async Task SendReportEmail(string emailAddress, string reportContent)
        {
            var subject = "Monthly Tour Report";

            var body = $@"
            <h1>Tour Report for {DateTime.Now.ToString("MMMM yyyy")}</h1>
            <p>{reportContent}</p>
        ";

            try
            {
                // Call the EmailService to send the email
                _emailService.SendEmail(emailAddress, subject, body);
                Console.WriteLine($"Report email sent to {emailAddress}");
            }
            catch (Exception ex)
            {
                // Log any exception that occurs while sending the email
                Console.WriteLine($"Error sending email to {emailAddress}: {ex.Message}");
            }
        }        

        public void SendTestEmail()
        {
            var tour = _tourRepository.GetTourByID(1); // Fetch tours happening in the next 48 hours
            var email = "sergej.vlaskalic@gmail.com";
            var emailBody = GenerateTourReminderEmailBody(tour); // Generate email content
            _emailService.SendEmail(email, "Reminder: Upcoming Tour", emailBody);
        }*/

        public Result CreateTourRate(TourRateDto tourRateDto)
        {

            // Call the repository to create the TourRate
            var createdTourRate = _tourRepository.CreateTourRate(tourRateDto);

            // Check if the creation was successful
            if (createdTourRate != null)
            {
                return Result.Ok();
            }

            // If creation failed, return a failure result
            return Result.Fail("Failed to create tour rate!");
        }

        public Result<TourRateDto>? GetTourRateByUser(long userId, long tourId)
        {
            var tourRate = _tourRepository.GetTourRateByUser(userId, tourId);

            if (tourRate == null)
            {
                return Result.Ok(); // Return an empty successful result
            }

            var dto = tourRate.MapToDto();            

            return Result.Ok(dto); // Return the DTO wrapped in a successful result
        }

        public bool CheckIfTourGuide(long guideId, long tourId)
        {
            return _tourRepository.CheckIfTourGuide(guideId, tourId);
        }

        public Result<List<TourRateDto>> GetTourRates(long tourId)
        {
            var tourRates = _tourRepository.GetTourRates(tourId);

            var tourRateDtos = tourRates.Select(tr => new TourRateDto
            {
                Id = tr.Id,
                TouristId = tr.TouristId,
                TourId = tr.TourId,
                Rating = tr.Rating,
                Comment = tr.Comment,
                TouristUsername = tr.TouristUsername
            }).ToList();

            return Result.Ok(tourRateDtos);
        }



        public int GetTourRateCount(long tourId)
        {
            var tourRates = _tourRepository.GetTourRates(tourId);
            return tourRates.Count;
        }

        public Result<TourCreateDto> GetBestRatedTourByGuideID(long guideID)
        {
            var tours = _tourRepository.GetGuideTours(guideID);
            if (tours == null || !tours.Any())
            {
                return Result.Fail("No tours found for this guide.");
            }

            Tour bestRatedTour = null;
            double highestRating = double.MinValue;

            foreach (var tour in tours)
            {
                var averageRating = _tourRepository.GetAverageTourRate(tour.Id);
                if (averageRating > highestRating)
                {
                    highestRating = averageRating;
                    bestRatedTour = tour;
                }
            }

            if (bestRatedTour == null)
            {
                return Result.Fail("No ratings available for any tours.");
            }

            var bestRatedTourDto = new TourCreateDto
            {
                Id = bestRatedTour.Id,
                Name = bestRatedTour.Name,
                Description = bestRatedTour.Description,
                guideId = bestRatedTour.GuideId,
                AverageRating = highestRating
            };

            return Result.Ok(bestRatedTourDto);
        }

        public Result<TourCreateDto> GetLowestRatedTourByGuideID(long guideID)
        {
            var tours = _tourRepository.GetGuideTours(guideID);
            if (tours == null || !tours.Any())
            {
                return Result.Fail("No tours found for this guide.");
            }

            Tour lowestRatedTour = null;
            double lowestRating = double.MaxValue;

            foreach (var tour in tours)
            {
                var averageRating = _tourRepository.GetAverageTourRate(tour.Id);
                if (averageRating < lowestRating)
                {
                    lowestRating = averageRating;
                    lowestRatedTour = tour;
                }
            }

            if (lowestRatedTour == null)
            {
                return Result.Fail("No ratings available for any tours.");
            }

            var lowestRatedTourDto = new TourCreateDto
            {
                Id = lowestRatedTour.Id,
                Name = lowestRatedTour.Name,
                Description = lowestRatedTour.Description,
                guideId = lowestRatedTour.GuideId,
                AverageRating = lowestRating
            };

            return Result.Ok(lowestRatedTourDto);
        }


        public GuideReportDto GetMonthlyGuideTourReport(long guideID, int year, int month)
        {
            var tours = _tourRepository.GetLastMonthGuideTours(guideID);
            if (tours == null || !tours.Any())
            {
                return null; // Ili Result.Fail("Nema dostupnih tura za vodiča.");
            }

            var report = new GuideReportDto
            {
                GuideId = guideID,
                Year = year,
                Month = month,
                SoldTours = new List<SoldTourDto>()
            };

            Tour bestRatedTour = null, worstRatedTour = null;
            double highestRating = double.MinValue, lowestRating = double.MaxValue;
            int bestRatedCount = 0, worstRatedCount = 0;

            int totalSalesCount = 0;

            foreach (var tour in tours)
            {
                // Broj prodatih tura u zadatom mesecu
                int soldCount = _tourRepository.GetTourPurchaseCount(tour.Id);
                report.SoldTours.Add(new SoldTourDto
                {
                    TourId = tour.Id,
                    TourName = tour.Name,
                    SalesCount = soldCount
                });
                totalSalesCount += soldCount;

                // Provera prosečne ocene ture
                double averageRating = _tourRepository.GetAverageTourRate(tour.Id);
                int ratingCount = this.GetTourRateCount(tour.Id);

                // Najbolje ocenjena tura
                if (averageRating > highestRating)
                {
                    highestRating = averageRating;
                    bestRatedTour = tour;
                    bestRatedCount = ratingCount;
                }

                // Najgore ocenjena tura
                if (averageRating < lowestRating)
                {
                    lowestRating = averageRating;
                    worstRatedTour = tour;
                    worstRatedCount = ratingCount;
                }
            }
            report.totalSales = totalSalesCount;

            if (bestRatedTour != null)
            {
                report.BestRatedTour = new RatedTourDto
                {
                    TourId = bestRatedTour.Id,
                    TourName = bestRatedTour.Name,
                    AverageRating = highestRating,
                    RatingCount = bestRatedCount
                };
            }

            if (worstRatedTour != null)
            {
                report.WorstRatedTour = new RatedTourDto
                {
                    TourId = worstRatedTour.Id,
                    TourName = worstRatedTour.Name,
                    AverageRating = lowestRating,
                    RatingCount = worstRatedCount
                };
            }

            return report;
        }


        /*
        private string GenerateTourReminderEmailBody(Tour tour)
        {
            return $"Dear Customer,\n\n" +
                   $"This is a reminder for your upcoming tour!\n\n" +
                   $"Tour: {tour.Name}\n" +
                   $"Date: {tour.Date.ToString("yyyy-MM-dd HH:mm:ss")}\n" +
                   $"Difficulty: {tour.Difficulty}\n" +
                   $"Description: {tour.Description}\n\n" +
                   $"We look forward to having you with us!\n\n" +
                   $"Best regards,\nYour Tour Service Team";
        }*/

        private string GenerateTourReminderEmailBody(Tour tour)
        {
            return $"<html>" +
                   "<body>" +
                   "<p>Dear Customer,</p>" +
                   "<p>This is a reminder for your upcoming tour!</p>" +
                   "<table>" +
                   $"<tr><td><strong>Tour:</strong></td><td>{tour.Name}</td></tr>" +
                   $"<tr><td><strong>Date:</strong></td><td>{tour.Date.ToString("yyyy-MM-dd HH:mm:ss")}</td></tr>" +
                   $"<tr><td><strong>Difficulty:</strong></td><td>{tour.Difficulty}</td></tr>" +
                   $"<tr><td><strong>Description:</strong></td><td>{tour.Description}</td></tr>" +
                   "</table>" +
                   "<p>We look forward to having you with us!</p>" +
                   "<p>Best regards,<br>Your Tour Service Team</p>" +
                   "</body>" +
                   "</html>";
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

        public Result CancelTour(long id)
        {           
            _tourRepository.CancelTour(id);

            return Result.Ok();
        }

        
        public Result BuyTours(long userId, List<long> tourIds)
        {
            /*var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return Result.Fail("User not found.");
            }*/

            var toursToBuy = _tourRepository.GetToursByIds(tourIds);
            if (toursToBuy == null || !toursToBuy.Any())
            {
                return Result.Fail("No valid tours found.");
            }

            // Example check if the user has enough bonus points to buy the tours
            /*
            if (user.BonusPoints < toursToBuy.Sum(t => t.Price)) // Assume the price is in bonus points
            {
                return Result.Fail("Not enough bonus points.");
            }*/

            // Deduct the points and mark tours as purchased
            //user.BonusPoints -= toursToBuy.Sum(t => t.Price); // Subtract the cost of the tours

            // Save the user purchase history
            foreach (var tour in toursToBuy)
            {
                var purchase = new UserTourPurchase(userId, tour.Id, DateTime.UtcNow);
                _tourRepository.CreatePurchase(purchase);
            }

            //_unitOfWork.Commit(); // Commit changes

            return Result.Ok();
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
                guideId = tour.GuideId,
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
                guideId = tour.GuideId,
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

