using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
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
        public EventService(ICrudRepository<Event> repository, IImageRepository imageRepository, IMapper mapper) : base(repository, mapper)
        {
            _eventRepository = repository;
            _imageRepository = imageRepository;
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
                dto.StartDate.ToUniversalTime();
                dto.EndDate.ToUniversalTime();
                Event tourEvent = MapToDomain(dto);
                tourEvent.StartDate = dto.StartDate.ToUniversalTime();
                tourEvent.EndDate = dto.EndDate.ToUniversalTime();

                if (dto.Image != null && !_imageRepository.Exists(dto.Image.Data))
                {
                    var newImage = new Image(
                        dto.Image.Data,
                        dto.Image.UploadedAt,
                        dto.Image.MimeType
                    );
                    _imageRepository.Create(newImage);

                    tourEvent.ImageId = newImage.Id;
                    tourEvent.Image = newImage;
                }
                else if (dto.Image != null && _imageRepository.Exists(dto.Image.Data))
                {
                    return Result.Fail(FailureCode.Conflict).WithError("Image already exists!");
                }
                _eventRepository.Create(tourEvent);
                return MapToDto(tourEvent);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }

        }
    }
}
