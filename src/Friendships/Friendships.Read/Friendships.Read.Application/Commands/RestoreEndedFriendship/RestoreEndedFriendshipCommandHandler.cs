using Dapper;
using Friendships.Read.Application.Contracts;
using Friendships.Read.Application.Logging;
using MediatR;
using Serilog;
using System.Data;

namespace Friendships.Read.Application.Commands.RestoreEndedFriendship;

public sealed class RestoreEndedFriendshipCommandHandler(
    IConnectionFactory connectionFactory,
    ILogger logger) 
    : IRequestHandler<RestoreEndedFriendshipCommand>
{
    public async Task Handle(
        RestoreEndedFriendshipCommand request,
        CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.Create();
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", request.UserId, DbType.Guid);
        parameters.Add("@FriendId", request.FriendId, DbType.Guid);

        var result = await connection.ExecuteScalarAsync<int>(
            "[dbo].[RestoreEndedFriendship]",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        switch (result)
        {
            case 0:
                RestoreEndedFriendshipLogMessages.LogSuccessfulRestoreEndedFriendship(logger, request);
                break;
            case 1:
                RestoreEndedFriendshipLogMessages.LogFriendshipNotFound(
                    logger, result, request.UserId, request.FriendId);
                break;
            case 2:
                RestoreEndedFriendshipLogMessages.LogUnexpectedError(logger, result, request);
                break;
            default:
                RestoreEndedFriendshipLogMessages.LogUnhandledErrorCode(logger, result, request);
                break;
        }
    }
}