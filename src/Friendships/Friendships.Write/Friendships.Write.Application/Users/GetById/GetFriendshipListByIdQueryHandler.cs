using ErrorOr;
using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Ids;
using MediatR;

namespace Friendships.Write.Application.Users.GetById;

public sealed class GetFriendshipListByIdQueryHandler : IRequestHandler<GetFriendshipListByUserIdQuery, ErrorOr<GetFriendshipListByUserIdResult>>
{
    private readonly IFriendshipListRepository _userFriendsRepository;

    public GetFriendshipListByIdQueryHandler(IFriendshipListRepository userFriendsRepository) => _userFriendsRepository = userFriendsRepository;

    public async Task<ErrorOr<GetFriendshipListByUserIdResult>> Handle(GetFriendshipListByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);

        var userFriendsResult = await _userFriendsRepository.GetFriendshipListByUserIdAsync(userId);

        if (userFriendsResult is null)
            return Errors.Entities.FriendshipList.NotFound(userId);

        return new GetFriendshipListByUserIdResult(userFriendsResult);
    }
}
