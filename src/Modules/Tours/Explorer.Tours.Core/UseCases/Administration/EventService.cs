using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class EventService : CrudService<EventDto, Event>, IEventService
    {
        private readonly ICrudRepository<Event> _eventRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ICrudRepository<Tour> _tourRepository;
        private readonly IMapper _mapper;
        public EventService(ICrudRepository<Event> repository, IImageRepository imageRepository, ICrudRepository<Tour> tourRepository, IMapper mapper) : base(repository, mapper)
        {
            _eventRepository = repository;
            _imageRepository = imageRepository;
            _tourRepository = tourRepository;
            _mapper = mapper;
        }
        public PagedResult<EventDto> GetAll()
        {
            var allEvents = _eventRepository.GetPaged(1, int.MaxValue);


            var events = allEvents.Results
                               .Select(obj => MapToDto(obj))
                               .ToList();

            return new PagedResult<EventDto>(events, events.Count());
        }

        public Result<EventDto> Create(EventDto dto)
        {
            try
            {
                var category = (EventCategory)Enum.Parse(typeof(EventCategory), dto.Category, true);
                dto.StartDate.ToUniversalTime();
                dto.EndDate.ToUniversalTime();
                var ev = new Event(dto.Name, dto.Description, category, dto.Longitude, dto.Latitude, dto.StartDate.ToUniversalTime(), dto.EndDate.ToUniversalTime(),
                    new Image(dto.Image.Data, dto.Image.UploadedAt, dto.Image.MimeType));
                
                var res = _eventRepository.Create(ev);
                return MapToDto(res);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }

        }

        public PagedResult<EventDto> GetAllEventsWithinRange(int v, double longitude, double latitude)
        {
            var allEvents = _eventRepository.GetPaged(1, int.MaxValue);


            var events = allEvents.Results
                               .Select(obj => MapToDto(obj))
                               .ToList();
           

            var eventsInRange = events.Where(e => e.CheckRadius(longitude, latitude)).ToList();


            return new PagedResult<EventDto>(eventsInRange, eventsInRange.Count());
        }

        public Result <EventDto> SubscribeToEvent(EventDto eventDto, int userId)
        {

            Event e = _eventRepository.Get(eventDto.Id);


            e.EventAcceptances.Add(new EventAcception(userId, DateTime.UtcNow));
            var result = _eventRepository.Update(e);
            return MapToDto(result);
            
        }

        public PagedResult<EventDto> GetSortedByAcceptance()
        {

            var allEvents = _eventRepository.GetPaged(1, int.MaxValue);
            var sortedEvents = allEvents.Results
               .OrderByDescending(e => e.EventAcceptances.Count)
               .ToList();

            var events = new List<EventDto>();
            foreach(Event e in sortedEvents)
            {
                events.Add(MapToDto(e));
            }


            return new PagedResult<EventDto>(events, events.Count());
        }
        public PagedResult<EventDto> GetTopThree()
        {

            var allEvents = _eventRepository.GetPaged(1, int.MaxValue);
            var sortedEvents = allEvents.Results
               .OrderByDescending(e => e.EventAcceptances.Count)
               .ToList();

            var events = new List<EventDto>();
            int counter = 0;
            foreach (Event e in sortedEvents)
            {
                if(counter < 3)
                {
                    events.Add(MapToDto(e));
                    counter++;
                }
            }

            return new PagedResult<EventDto>(events, events.Count());
        }


        public Result Delete(long id)
        {
            try
            {
                _eventRepository.Delete(id);
                return Result.Ok();
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result Update(EventDto dto, int userId)
        {

            var tourEvent = _eventRepository.Get(dto.Id);
            if (tourEvent == null)
            {
                return Result.Fail(FailureCode.NotFound).WithError("Event not found!");
            }

            tourEvent.StartDate = dto.StartDate.ToUniversalTime();
            tourEvent.EndDate = dto.EndDate.ToUniversalTime();

            tourEvent.Name = dto.Name;
            tourEvent.Description = dto.Description;
            tourEvent.Category = Enum.Parse<EventCategory>(dto.Category);

  
            _eventRepository.Update(tourEvent);
            return Result.Ok();
        }


            public PagedResult<TourDto> GetNearTours(long eventId)
        {
            var tours = _tourRepository.GetPaged(1, int.MaxValue).Results;
            var eventt = _eventRepository.Get(eventId);

            var nearbyTours = tours.Where(tour =>
            {
                var firstCheckpoint = tour.Checkpoints.FirstOrDefault();
                if (firstCheckpoint == null)
                {
                    return false;
                }

                double distance = CalculateDistance(eventt.Latitude, eventt.Longitude,
                                                    firstCheckpoint.Latitude, firstCheckpoint.Longitude);
                return distance <= 30000;
            }).ToList();
            var dtos = nearbyTours.Select(t => _mapper.Map<TourDto>(t)).ToList();
            return new PagedResult<TourDto>(dtos.ToList(), nearbyTours.Count());
        }
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;

            double latRad1 = lat1 * (Math.PI / 180);
            double latRad2 = lat2 * (Math.PI / 180);
            double deltaLat = (lat2 - lat1) * (Math.PI / 180);
            double deltaLon = (lon2 - lon1) * (Math.PI / 180);

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(latRad1) * Math.Cos(latRad2) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        public PagedResult<EventDto> FindAllByCategories(List<EventCategory> categories)
        {
            var allEvents = _eventRepository.GetPaged(1, int.MaxValue);


            var events = allEvents.Results
                              .Select(obj => MapToDto(obj))
                              .ToList();


            var today = DateTime.UtcNow.Date;
            var next7Days = today.AddDays(7);

            var filteredEvents = events
                .Where(e =>
                    categories.Contains((EventCategory)Enum.Parse(typeof(EventCategory), e.Category)) &&
                    (
                        (e.StartDate >= today && e.StartDate <= next7Days) || (e.EndDate >= today && e.StartDate <= next7Days)      
                    )
                )
                .ToList();
            return new PagedResult<EventDto>(filteredEvents, filteredEvents.Count());




        }
    }
}
