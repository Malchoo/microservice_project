using Dapper;
using Friendships.Read.Application.Contracts;
using Friendships.Read.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Friendships.Read.Api.Endpoints;

public static class ActiveFriendshipsEndpoints
{
    public static void MapActiveFriendshipsEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("activeFriendships");

        group.MapGet("/count/{userId}", GetUserActiveFriendshipsCount);
        group.MapGet("/{userId}", GetUserActiveFriendships);
        group.MapGet("/friendIds/{userId}", GetUserActiveFriendIds);
        group.MapGet("/type/{userId}/{friendId}", GetActiveFriendshipType);
        group.MapGet("/byType/{userId}/{friendshipType}", GetUserActiveFriendshipsByType);
        group.MapGet("/friendIdsByType/{userId}/{friendshipType}", GetUserActiveFriendIdsByType);
        group.MapGet("/invitation/{invitationId}", GetActiveFriendshipByInvitation);
        group.MapGet("/areFriends/{userId}/{friendId}", AreUsersFriends);
    }

    private static IDbConnection CreateConnection(IConnectionFactory connectionFactory)
        => connectionFactory.Create();
    
    private static async Task<IResult> GetUserActiveFriendshipsCount(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var count = await connection.QueryFirstOrDefaultAsync<int?>(
            "[dbo].[GetUserActiveFriendshipsCount]",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);

        return count.HasValue ? Results.Ok(count.Value) : Results.Ok(0);
    }

    private static async Task<IResult> GetUserActiveFriendships(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendships = await connection.QueryAsync<FriendshipDto>(
            "[dbo].[GetUserActiveFriendships]",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);

        return Results.Ok(friendships);
    }

    private static async Task<IResult> GetUserActiveFriendIds(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendIds = await connection.QueryAsync<Guid>(
            "[dbo].[GetUserActiveFriendIds]",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);

        return Results.Ok(friendIds);
    }

    private static async Task<IResult> GetActiveFriendshipType(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        [FromRoute] Guid friendId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendshipType = await connection.QueryFirstOrDefaultAsync<int>(
            "[dbo].[GetActiveFriendshipType]",
            new { UserId = userId, FriendId = friendId },
            commandType: CommandType.StoredProcedure);

        return friendshipType != -1 ? Results.Ok(friendshipType) : Results.NotFound();
    }

    private static async Task<IResult> GetUserActiveFriendshipsByType(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        [FromRoute] int friendshipType,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendships = await connection.QueryAsync<FriendshipDto>(
            "[dbo].[GetUserActiveFriendshipsByType]",
            new { UserId = userId, FriendshipType = friendshipType },
            commandType: CommandType.StoredProcedure);

        return Results.Ok(friendships);
    }

    private static async Task<IResult> GetUserActiveFriendIdsByType(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        [FromRoute] int friendshipType,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendIds = await connection.QueryAsync<Guid>(
            "[dbo].[GetUserActiveFriendIdsByType]",
            new { UserId = userId, FriendshipType = friendshipType },
            commandType: CommandType.StoredProcedure);

        return Results.Ok(friendIds);
    }

    private static async Task<IResult> GetActiveFriendshipByInvitation(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid invitationId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var friendship = await connection.QueryFirstOrDefaultAsync<FriendshipDto>(
            "[dbo].[GetActiveFriendshipByInvitation]",
            new { InvitationId = invitationId },
            commandType: CommandType.StoredProcedure);

        return friendship != null ? Results.Ok(friendship) : Results.NotFound();
    }

    private static async Task<IResult> AreUsersFriends(
        [FromServices] IConnectionFactory connectionFactory,
        [FromRoute] Guid userId,
        [FromRoute] Guid friendId,
        CancellationToken ct)
    {
        using var connection = CreateConnection(connectionFactory);
        var areFriends = await connection.QueryFirstOrDefaultAsync<bool>(
            "[dbo].[AreUsersFriends]",
            new { UserId = userId, FriendId = friendId },
            commandType: CommandType.StoredProcedure);

        return Results.Ok(areFriends);
    }
}