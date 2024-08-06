using Dapper;
using ErrorOr;
using Friendships.Application.ApplicationErrors;
using Friendships.Write.Application.Dto;
using Friendships.Write.Domain.Enums;
using System.Data;

namespace Friendships.Write.Application.Queries;

public class Queries
{
    private readonly IDbConnection _connection;

    public Queries(IDbConnection connection)
    => _connection = connection;

    public async Task<ErrorOr<IsFriendDto>> IsFriendById(Guid userId, Guid friendId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId, DbType.Guid);
        parameters.Add("FriendId", friendId, DbType.Guid);

        var isFriend = await _connection.QuerySingleOrDefaultAsync<bool?>(
            "dbo.IsFriendById", parameters, commandType: CommandType.StoredProcedure);

        if (isFriend is null)
            return Errors.Dto.IsFriendDtoErrors.UserOrFriendNotFound;

        return new IsFriendDto(isFriend.Value);
    }

    public async Task<ErrorOr<FriendshipLevelDto>> GetFriendshipLevel(Guid userId, Guid friendId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId, DbType.Guid);
        parameters.Add("FriendId", friendId, DbType.Guid);

        var friendshipLevelResult = await _connection.QuerySingleOrDefaultAsync<string>(
            "dbo.GetFriendshipLevel", parameters, commandType: CommandType.StoredProcedure);

        if (friendshipLevelResult is null)
            return Errors.Dto.FriendshipLevelDto.FriendshipLevelNotFound;

        var friendshipLevel = FriendshipLevel.FromName(friendshipLevelResult);

        return new FriendshipLevelDto(friendshipLevel);
    }

    public async Task<ErrorOr<GetFriendsDto>> GetFriendsById(Guid userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId, DbType.Guid);

        var friends = await _connection.QueryAsync<Guid>(
            "dbo.GetFriendsById", parameters, commandType: CommandType.StoredProcedure);

        if (friends is null)
            return Errors.Dto.GetFriendsDto.FriendsNotFound;

        return new GetFriendsDto(friends.ToList());
    }
}
