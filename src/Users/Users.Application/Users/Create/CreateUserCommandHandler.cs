using ErrorOr;
using MediatR;
using Users.Contracts.Users.Create;
using Users.Domain.Contracts;
using Users.Domain.Entities;
using Users.Domain.Ids;
using Users.Domain.Time;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Application.Users.Create;

public class CreateUserCommandHandler :
    IRequestHandler<CreateUserCommand, ErrorOr<CreateUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISystemTimeProvider _systemTimeProvider;
    private readonly List<Error> _errors = [];

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        ISystemTimeProvider time)
    {
        _userRepository = userRepository;
        _systemTimeProvider = time;
    }

    public async Task<ErrorOr<CreateUserResponse>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userResult = await CreateUserAsync(command);

        if (userResult.IsError)
            return userResult.Errors;

        await _userRepository.AddUserAsync(userResult.Value);

        return new CreateUserResponse(userResult.Value.Id.Value);
    }

    private async Task<ErrorOr<User>> CreateUserAsync(CreateUserCommand command)
    {
        var registrationId = new RegistrationId(command.RegistrationId);

        var username = Create(command.Username, Username.Create);

        var email = Create(command.Email, Email.Create);

        var mobileNumber = Create(command.MobileNumber, MobileNumber.Create);

        var firstName = Create(command.FirstName, FirstName.Create);

        var middleName = Create(command.MiddleName, MiddleName.Create);

        var lastName = Create(command.LastName, LastName.Create);

        await CheckUserExistsAsync(email.Value, mobileNumber.Value, username.Value);

        if (_errors.Count > 0)
            return _errors;

        var userResult = User.Create(
            registrationId,
            username.Value,
            firstName.Value,
            middleName.Value,
            lastName.Value,
            email.Value,
            mobileNumber.Value,
            command.Currency,
            command.TwoFactorAuth,
            command.Title,
            command.Language,
            command.Theme);

        return userResult;
    }

    private ErrorOr<T> Create<T>(string value, Func<string, ErrorOr<T>> creator)
    {
        var result = creator(value);
        if (result.IsError)
        {
            _errors.AddRange(result.Errors);
            return result.Errors;
        }
        return result.Value;
    }

    private async Task CheckUserExistsAsync(Email email, MobileNumber mobileNumber, Username username)
    {
        //if (await _userRepository.ExistsByEmailAsync(email))
        //    _errors.Add(UserErrors.EmailHasBeenTaken(email));

        //if (await _userRepository.ExistsByMobileNumberAsync(mobileNumber))
        //    _errors.Add(UserErrors.MobileNumberHasBeenTaken(mobileNumber));

        //if (await _userRepository.ExistsByUsernameAsync(username))
        //    _errors.Add(UserErrors.UsernameHasBeenTaken(username));
    }
}
