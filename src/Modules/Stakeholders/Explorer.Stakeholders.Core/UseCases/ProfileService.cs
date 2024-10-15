using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class ProfileService : CrudService<ProfileDto, Person>, IProfileService
{
    private readonly IImageRepository _imageRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICrudRepository<Person> _personRepository;
    private readonly ITransactionRepository _transactionRepository;

    public ProfileService(IUserRepository userRepository,
        ICrudRepository<Person> personRepository,
        ITransactionRepository transactionRepository,
        IImageRepository imageRepository,
        IMapper mapper)
        : base(personRepository, mapper)
    {
        _userRepository = userRepository;
        _personRepository = personRepository;
        _transactionRepository = transactionRepository;
        _imageRepository = imageRepository;
    }

    public Result<ProfileDto> Get(long personId)
    {
        try
        {
            var person = _personRepository.Get(personId);

            return MapToDto(person);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result<ProfileDto> Update(long personId, ProfileDto profile)
    {
        try
        {
            var person = _personRepository.Get(personId);

            // Check if the username already exists
            if (_userRepository.GetActiveByName(profile.Username) != null && _userRepository.GetActiveByName(profile.Username) != person.User)
            {
                return Result.Fail(FailureCode.Conflict).WithError("Username already exists");
            }

            // Start a transaction
            _transactionRepository.BeginTransaction();

            // Update the user with the new username
            var user = person.User;
            user.Username = profile.Username;

            _userRepository.Update(user);

            // Create the image and save it
            if(profile.Image != null && !_imageRepository.Exists(profile.Image.Data))
            {
                // If the profile has an image, create a new image object with the data from the profile
                var newImage = new Image(
                    profile.Image.Data,
                    profile.Image.UploadedAt,
                    profile.Image.MimeType
                );

                // Save the new image to the repository
                _imageRepository.Create(newImage);

                // Update the person with the new image
                person.ImageId = newImage.Id;
                person.Image = newImage;
            } else if(profile.Image != null && _imageRepository.Exists(profile.Image.Data))
            {
                // If the image already exists, get the image from the repository
                var image = _imageRepository.GetByData(profile.Image.Data);

                // Update the person with the existing image
                person.ImageId = image.Id;
                person.Image = image;
            }

            // Update the person with all the necessary data
            person.Email = profile.Email;
            person.Name = profile.Name;
            person.Surname = profile.LastName;
            person.UserId = user.Id;
            person.User = user;
            person.Biography = profile.Biography;
            person.Moto = profile.Moto;

            var newPerson = _personRepository.Update(person);

            // Commit the transaction
            _transactionRepository.CommitTransaction();

            // Create the new DTO to return
            return MapToDto(newPerson);
        }
        catch (KeyNotFoundException e)
        {
            // If user or person is not found, rollback the transaction
            if(_transactionRepository.HasActiveTransacation()) _transactionRepository.RollbackTransaction();

            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            // If something goes wrong, rollback the transaction
            if (_transactionRepository.HasActiveTransacation()) _transactionRepository.RollbackTransaction();

            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }
}
