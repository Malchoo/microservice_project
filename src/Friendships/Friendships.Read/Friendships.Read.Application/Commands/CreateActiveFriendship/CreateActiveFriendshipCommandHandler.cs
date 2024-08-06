using Dapper;
using Friendships.Read.Application.Contracts;
using Friendships.Read.Application.Logging;
using MediatR;
using Serilog;
using System.Data;

namespace Friendships.Read.Application.Commands.CreateActiveFriendship;

public sealed class CreateActiveFriendshipCommandHandler(
    IConnectionFactory connectionFactory,
    ILogger logger)
    : IRequestHandler<CreateActiveFriendshipCommand>
{
    public async Task Handle(
        CreateActiveFriendshipCommand request, 
        CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.Create();
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", request.UserId, DbType.Guid);
        parameters.Add("@FriendId", request.FriendId, DbType.Guid);
        parameters.Add("@FriendshipType", request.FriendshipType, DbType.Int32);

        var result = await connection.ExecuteScalarAsync<int>(
            "[dbo].[CreateActiveFriendship]",
            parameters,
            commandType: CommandType.StoredProcedure);

        switch (result)
        {
            case 0:
                CreateActiveFriendshipLogMessages.LogSuccessfulCreation(logger, request);
                break;
            case 1:
                CreateActiveFriendshipLogMessages.LogOneOrBothUsersDoNotExist(logger, result, request.UserId, request.FriendId);
                break;
            case 2:
                CreateActiveFriendshipLogMessages.LogFriendshipAlreadyExists(logger, result, request.UserId, request.FriendId);
                break;
            case 3:
                CreateActiveFriendshipLogMessages.LogUnexpectedError(logger, result, request);
                break;
            default:
                CreateActiveFriendshipLogMessages.LogUnhandledErrorCode(logger, result, request);
                break;
        }
    }
}