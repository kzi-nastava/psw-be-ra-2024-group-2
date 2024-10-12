using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class ProfileService : CrudService<ProfileDto, Person>, IProfileService
{
    private readonly ICrudRepository<Image> _imageRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICrudRepository<Person> _personRepository;
    private readonly ITransactionRepository _transactionRepository;

    public ProfileService(ICrudRepository<Image> imageRepository,
        IUserRepository userRepository,
        ICrudRepository<Person> personRepository,
        ITransactionRepository transactionRepository,
        IMapper mapper)
        : base(personRepository, mapper)
    {
        _imageRepository = imageRepository;
        _userRepository = userRepository;
        _personRepository = personRepository;
        _transactionRepository = transactionRepository;
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

            if (profile.Image == null)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("Image is required");
            }

            // First: Start a transaction
            _transactionRepository.BeginTransaction();

            // Second: Update the user with the new username
            var user = person.User;
            user.Username = profile.Username;

            _userRepository.Update(user);

            // Third: Create the image and save it
            var newImage = new Image(
                profile.Image.Data,
                profile.Image.UploadedAt,
                profile.Image.MimeType
            );

            _imageRepository.Create(newImage);

            // Fourth: Update the person with all the necessary data
            person.Email = profile.Email;
            person.Name = profile.Name;
            person.Surname = profile.LastName;
            person.UserId = user.Id;
            person.User = user;
            person.ImageId = newImage.Id;
            person.Image = newImage;
            person.Biography = profile.Biography;
            person.Moto = profile.Moto;

            var newPerson = _personRepository.Update(person);

            if (newPerson.Id != personId)
            {
                throw new ArgumentException("PersonId does not match the updated person");
            }

            // Fifth: Commit the transaction
            _transactionRepository.CommitTransaction();

            // Sixth: Create the new DTO to return
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
