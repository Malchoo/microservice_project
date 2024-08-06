using ErrorOr;
using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Entities;
using Friendships.Write.Domain.Ids;
using MediatR;
using SharedKernel.IntegrationEvents.User;

namespace Friendships.Write.Application.IntegrationEvents;

public class UserCreatedIntegrationEventHandler : INotificationHandler<UserCreatedIntegrationEvent>
{
    private readonly IFriendshipListRepository _friendshipListRepository;
    private readonly List<Error> _errors = new();

    public UserCreatedIntegrationEventHandler(IFriendshipListRepository friendshipListRepository)
    {
        _friendshipListRepository = friendshipListRepository;
    }

    public async Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = new UserId(notification.UserId);

            var userFriendsResult = FriendshipList.Create(userId);

            if (userFriendsResult.IsError)
            {
                _errors.AddRange(userFriendsResult.Errors);
                return;
            }

            await _friendshipListRepository.AddFriendshipListAsync(userFriendsResult.Value);

        }
        catch (Exception ex)
        {

            throw new ArgumentException(ex.Message);
        }
    }
}
