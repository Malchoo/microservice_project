using ErrorOr;
using Friendships.Write.Contracts.Friendships.Create;
using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Ids;
using Friendships.Write.Domain.Time;
using MediatR;

namespace Friendships.Write.Application.Users.Create;

public class CreateFriendshipListCommandHandler :
    IRequestHandler<CreateFriendshipListCommand, ErrorOr<CreateFriendshipResponse>>
{
    private readonly IFriendshipListRepository _FriendshipListRepository;
    private readonly ISystemTimeProvider _systemTimeProvider;
    private readonly List<Error> _errors = new();

    public CreateFriendshipListCommandHandler(
        IFriendshipListRepository FriendshipListRepository,
        ISystemTimeProvider time)
    {
        _FriendshipListRepository = FriendshipListRepository;
        _systemTimeProvider = time;
    }

    public async Task<ErrorOr<CreateFriendshipResponse>> Handle(CreateFriendshipListCommand command, CancellationToken cancellationToken)
    {

        var userId = new UserId(command.UserId);

        var inviterId = new InviterId(command.InviterId);

        var invitationId = new InvitationId(command.InvitationId);

        await CheckUsersExistsAsync(userId, inviterId);

        if (_errors.Count > 0)
            return _errors;

        var allFriends = await _FriendshipListRepository.GetFriendshipListByUserIdAsync(userId);

        if (allFriends is null)
            return _errors;

        var allFriendsResult = allFriends.CreateFriendship(inviterId, invitationId);

        if (allFriendsResult.IsError)
            return allFriendsResult.Errors;

        await _FriendshipListRepository.UpdateFriendshipListAsync(allFriendsResult.Value);

        return new CreateFriendshipResponse(allFriendsResult.Value.Id.Value);
    }

    private async Task CheckUsersExistsAsync(UserId userId, InviterId inviterId)
    {
        if (!await _FriendshipListRepository.ExistsByUserIdlAsync(userId))
            _errors.Add(Friendships.Domain.DomainErrors.Errors.Entities.FriendshipList.NotFound(userId));

        if (!await _FriendshipListRepository.ExistsByUserIdlAsync(new UserId(inviterId.Value)))
            _errors.Add(Friendships.Domain.DomainErrors.Errors.Entities.FriendshipList.NotFound(inviterId));
    }
}
