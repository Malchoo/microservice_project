using Dapper;
using Friendships.Read.Application.Contracts;
using Friendships.Read.Application.Logging;
using MediatR;
using System.Data;
using Serilog;

namespace Friendships.Read.Application.Commands.EndActiveFriendship;

public sealed class EndActiveFriendshipCommandHandler(
    IConnectionFactory connectionFactory,
    ILogger logger)
    : IRequestHandler<EndActiveFriendshipCommand>
{
    public async Task Handle(
        EndActiveFriendshipCommand request,
        CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.Create();
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", request.UserId, DbType.Guid);
        parameters.Add("@FriendId", request.FriendId, DbType.Guid);

        var result = await connection.ExecuteScalarAsync<int>(
            "[dbo].[EndActiveFriendship]",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        switch (result)
        {
            case 0:
                EndActiveFriendshipLogMessages.LogSuccessfulEndActiveFriendship(logger, request);
                break;
            case 1:
                EndActiveFriendshipLogMessages.LogFriendshipNotFound(
                    logger, result, request.UserId, request.FriendId);
                break;
            case 2:
                EndActiveFriendshipLogMessages.LogUnexpectedError(logger, result, request);
                break;
            default:
                EndActiveFriendshipLogMessages.LogUnhandledErrorCode(logger, result, request);
                break;
        }
    }
}