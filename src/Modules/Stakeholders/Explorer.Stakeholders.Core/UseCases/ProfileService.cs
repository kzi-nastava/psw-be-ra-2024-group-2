using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class ProfileService : CrudService<ProfileDto, Person>, IProfileService
{
    private readonly ICrudRepository<Image> _imageRepository;
    private readonly ICrudRepository<Person> _personRepository;

    public ProfileService(ICrudRepository<Image> imageRepository, ICrudRepository<Person> personRepository, IMapper mapper)
        : base(personRepository, mapper)
    {
        _imageRepository = imageRepository;
        _personRepository = personRepository;
    }

    public Result<ProfileDto> Get(long personId)
    {
        var person = _personRepository.Get(personId);

        if (person == null)
        {
            return Result.Fail(new Error("Person not found"));
        }

        return MapToDto(person);
    }

    public Result<ProfileDto> Update(long personId, ProfileDto profile)
    {
        var person = _personRepository.Get(personId);

        if (person == null)
        {
            return Result.Fail(new Error("Person not found"));
        }

        if(profile.Image == null)
        {
            return Result.Fail(new Error("Image is required"));
        }

        var newImage = new Image(
            profile.Image.Data,
            profile.Image.UploadedAt,
            profile.Image.MimeType
        );

        try
        {
            var updatedImage = _imageRepository.Create(newImage);

            var newPerson = new Person(
                person.UserId,
                profile.Name,
                profile.LastName,
                profile.Email,
                profile.Biography,
                profile.Moto,
                updatedImage.Id
            );
            
            person.Update(newPerson, updatedImage);

            var updatedPerson = _personRepository.Update(newPerson);

            return MapToDto(updatedPerson);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(new Error(e.Message));
        }
    }
}
