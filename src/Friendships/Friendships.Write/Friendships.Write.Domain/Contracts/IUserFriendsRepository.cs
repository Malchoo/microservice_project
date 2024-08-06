using Friendships.Write.Domain.Entities;
using Friendships.Write.Domain.Ids;

namespace Friendships.Write.Domain.Contracts;
public interface IFriendshipListRepository
{
    Task AddFriendshipListAsync(FriendshipList userFriends);
    Task UpdateFriendshipListAsync(FriendshipList userFriends);
    Task<FriendshipList?> GetFriendshipListByUserIdAsync(UserId userId);
    Task<bool> ExistsByUserIdlAsync(UserId userId);
}