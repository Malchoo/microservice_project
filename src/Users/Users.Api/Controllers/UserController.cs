using Ardalis.SmartEnum;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Errors;
using Users.Application.Users.Create;
using Users.Application.Users.GetById;
using Users.Contracts.Users.Create;
using Users.Contracts.Users.Enums;

namespace Users.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ApiController
{
    private readonly ISender _sender;
    private readonly List<Error> _errors = [];

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("/create")]
    public async Task<IActionResult> CreateUserAsync(CreateUserRequest request)
    {
        var commandResult = CreateCommand(request);

        if (commandResult.IsError)
            return Problem(commandResult.Errors);

        var createResult = await _sender.Send(commandResult.Value);

        return createResult.Match(base.Ok, Problem);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserByUserId(Guid userId)
    {
        var userIdQuery = new GetUserByUserIdQuery(userId);
        var userResult = await _sender.Send(userIdQuery);

        return userResult.Match(base.Ok, Problem);
    }

    private ErrorOr<CreateUserCommand> CreateCommand(CreateUserRequest request)
    {
        var currencyResult = CreateSmartEnum<Currency, Domain.Enums.Currency>(request.Currency);
        var twoFactoryAuthResult = CreateSmartEnum<TwoFactorAuth, Domain.Enums.TwoFactorAuth>(request.TwoFactorAuth);
        var titleResult = CreateSmartEnum<Title, Domain.Enums.Title>(request.Title);
        var languageResult = CreateSmartEnum<Language, Domain.Enums.Language>(request.Language);
        var themeResult = CreateSmartEnum<Theme, Domain.Enums.Theme>(request.Theme);

        if (_errors.Count != 0)
            return _errors;

        return new CreateUserCommand(
            request.RegistrationId,
            request.Username,
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.Email,
            request.MobileNumber,
            currencyResult.Value,
            twoFactoryAuthResult.Value,
            titleResult.Value,
            languageResult.Value,
            themeResult.Value);
    }

    private ErrorOr<TSmartEnum> CreateSmartEnum<TEnum, TSmartEnum>(TEnum value)
        where TEnum : struct, Enum
        where TSmartEnum : SmartEnum<TSmartEnum>
    {
        if (SmartEnum<TSmartEnum>.TryFromName(value.ToString(), out TSmartEnum result))
            return result;

        _errors.Add(UserControllerErrors.CannotConvertEnum(typeof(TEnum).Name));

        return _errors.Last();
    }
}