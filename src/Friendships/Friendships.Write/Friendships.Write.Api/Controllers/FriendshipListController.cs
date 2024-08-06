using Ardalis.SmartEnum;
using ErrorOr;
using Friendships.Write.Api.Errors;
using Friendships.Write.Application.Users.Create;
using Friendships.Write.Application.Users.GetById;
using Friendships.Write.Contracts.Friendships.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Friendships.Write.Contracts.Friendships.Enums;


namespace Friendships.Write.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class FriendshipListController : ApiController
{
    private readonly ISender _sender;
    private readonly List<Error> _errors = new();

    public FriendshipListController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("/createFriendship")]
    public async Task<IActionResult> CreateFriendshipAsync(CreateFriendshipRequest request)
    {
        var commandResult = CreateCommand(request);

        if (commandResult.IsError)
            return Problem(commandResult.Errors);

        var createResult = await _sender.Send(commandResult.Value);

        return createResult.Match(base.Ok, Problem);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetFriendshipsListAsync(Guid userId)
    {
        var query = new GetFriendshipListByUserIdQuery(userId);
        var queryResult = await _sender.Send(query);
        return queryResult.Match(base.Ok, Problem);
    }

    private ErrorOr<CreateFriendshipListCommand> CreateCommand(CreateFriendshipRequest request)
    {
        var friendshipLevelResult = CreateSmartEnum<FriendshipLevel, Domain.Enums.FriendshipLevel>(request.FriendshipLevel);

        if (_errors.Count != 0)
            return _errors;

        return new CreateFriendshipListCommand(
            request.UserId,
            request.FriendId,
            request.InvitationId,
            friendshipLevelResult.Value);
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