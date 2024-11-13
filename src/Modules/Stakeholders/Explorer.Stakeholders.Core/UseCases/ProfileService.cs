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
    private readonly IMapper _mapper;

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
        _mapper = mapper;
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
            if (profile.Image != null && !_imageRepository.Exists(profile.Image.Data))
            {
                var newImage = new Image(
                    profile.Image.Data,
                    profile.Image.UploadedAt,
                    profile.Image.MimeType
                );

                _imageRepository.Create(newImage);
                person.ImageId = newImage.Id;
                person.Image = newImage;
            }
            else if (profile.Image != null && _imageRepository.Exists(profile.Image.Data))
            {
                var image = _imageRepository.GetByData(profile.Image.Data);
                person.ImageId = image.Id;
                person.Image = image;
            }

            person.Email = profile.Email;
            person.Name = profile.Name;
            person.Surname = profile.LastName;
            person.UserId = user.Id;
            person.User = user;
            person.Biography = profile.Biography;
            person.Moto = profile.Moto;

            var newPerson = _personRepository.Update(person);

            _transactionRepository.CommitTransaction();

            return MapToDto(newPerson);
        }
        catch (KeyNotFoundException e)
        {
            if (_transactionRepository.HasActiveTransacation()) _transactionRepository.RollbackTransaction();
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            if (_transactionRepository.HasActiveTransacation()) _transactionRepository.RollbackTransaction();
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }
    public Result<List<ProfileDto>> GetAllUsers()
    {
        try
        {
            var users = _userRepository.GetAll();
            return Result.Ok(users.Select(user => new ProfileDto
            {
                Username = user.Username,
                Id = user.Id,
                // Ostala polja će biti null jer nisu dostupna u User klasi
                Name = null,
                LastName = null,
                Email = null,
                Biography = null,
                Moto = null,
                Image = null
            }).ToList());
        }
        catch (Exception e)
        {
            return Result.Fail(FailureCode.Internal).WithError(e.Message);
        }
    }



}
