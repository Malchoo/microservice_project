using Dapper;
using Friendships.Read.Application.Contracts;
using Friendships.Read.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Friendships.Read.Api.Endpoints;

public static class EndedFriendshipsEndpoints
{
    public static void MapEndedFriendshipsEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("endedFriendships");

        group.MapGet("/count/{userId}", GetUserEndedFriendshipsCount);
        group.MapGet("/{userId}", GetUserEndedFriendships);
        group.MapGet("/friendIds/{userId}", GetUserEndedFriendIds);
        group.MapGet("/type/{userId}/{friendId}", GetEndedFriendshipType);
        group.MapGet("/byType/{userId}/{friendshipType}", GetUserEndedFriendshipsByType);
        group.MapGet("/friendIdsByType/{userId}/{friendshipType}", GetUserEndedFriendIdsByType);
        group.MapGet("/haveEnded/{userId}/{friendId}", HaveUsersEndedFriendship);
    }

    private static IDbConnection CreateConnection(IConnectionFactory connectionFactory)
        => connectionFactory.Create();

    private static async Task<IResult> GetUserEndedFriendshipsCount(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var count = await connection.QueryFirstOrDefaultAsync<int?>(
            "[dbo].[GetUserEndedFriendshipsCount]",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);

        return count.HasValue ? Results.Ok(count.Value) : Results.Ok(0);
    }

    private static async Task<IResult> GetUserEndedFriendships(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendships = await connection.QueryAsync<FriendshipDto>(
            "[dbo].[GetUserEndedFriendships]",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);

        return Results.Ok(friendships);
    }

    private static async Task<IResult> GetUserEndedFriendIds(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendIds = await connection.QueryAsync<Guid>(
            "[dbo].[GetUserEndedFriendIds]",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);

        return Results.Ok(friendIds);
    }

    private static async Task<IResult> GetEndedFriendshipType(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        [FromRoute] Guid friendId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendshipType = await connection.QueryFirstOrDefaultAsync<int>(
            "[dbo].[GetEndedFriendshipType]",
            new { UserId = userId, FriendId = friendId },
            commandType: CommandType.StoredProcedure);

        return friendshipType != -1 ? Results.Ok(friendshipType) : Results.NotFound();
    }

    private static async Task<IResult> GetUserEndedFriendshipsByType(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        [FromRoute] int friendshipType,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendships = await connection.QueryAsync<FriendshipDto>(
            "[dbo].[GetUserEndedFriendshipsByType]",
            new { UserId = userId, FriendshipType = friendshipType },
            commandType: CommandType.StoredProcedure);

        return Results.Ok(friendships);
    }

    private static async Task<IResult> GetUserEndedFriendIdsByType(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        [FromRoute] int friendshipType,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendIds = await connection.QueryAsync<Guid>(
            "[dbo].[GetUserEndedFriendIdsByType]",
            new { UserId = userId, FriendshipType = friendshipType },
            commandType: CommandType.StoredProcedure);

        return Results.Ok(friendIds);
    }

    private static async Task<IResult> HaveUsersEndedFriendship(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        [FromRoute] Guid friendId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var haveEnded = await connection.QueryFirstOrDefaultAsync<bool>(
            "[dbo].[HaveUsersEndedFriendship]",
            new { UserId = userId, FriendId = friendId },
            commandType: CommandType.StoredProcedure);

        return Results.Ok(haveEnded);
    }
}