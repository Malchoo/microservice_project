using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Entities;
using Friendships.Write.Domain.Ids;
using Microsoft.EntityFrameworkCore;

namespace Friendships.Write.Infrastructure.Persistence.Repositories;

public class FriendshipListRepository(FriendshipListDbContext dbContext) : IFriendshipListRepository
{
    private readonly FriendshipListDbContext _dbContext = dbContext;

    public async Task AddFriendshipListAsync(FriendshipList userFriends)
    {
        await _dbContext.FriendshipList.AddAsync(userFriends);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FriendshipList?> GetFriendshipListByUserIdAsync(UserId userId)
    {
        return await _dbContext.FriendshipList.FirstOrDefaultAsync(uF => uF.Id == userId);
    }

    public async Task UpdateFriendshipListAsync(FriendshipList userFriends)
    {
        _dbContext.FriendshipList.Update(userFriends);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsByUserIdlAsync(UserId userId)
    {
        return await _dbContext.FriendshipList.AnyAsync(user => user.Id == userId);
    }
}


