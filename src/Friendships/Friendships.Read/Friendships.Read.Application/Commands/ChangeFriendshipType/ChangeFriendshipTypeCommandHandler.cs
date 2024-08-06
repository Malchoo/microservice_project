using Dapper;
using Friendships.Read.Application.Contracts;
using Friendships.Read.Application.Logging;
using MediatR;
using Serilog;
using System.Data;

namespace Friendships.Read.Application.Commands.ChangeFriendshipType;

public sealed class ChangeFriendshipTypeCommandHandler(
    IConnectionFactory connectionFactory,
    ILogger logger) 
    : IRequestHandler<ChangeFriendshipTypeCommand>
{
    public async Task Handle(
        ChangeFriendshipTypeCommand request, 
        CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.Create();
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", request.UserId, DbType.Guid);
        parameters.Add("@FriendId", request.FriendId, DbType.Guid);
        parameters.Add("@NewFriendshipType", request.NewFriendshipType, DbType.Int32);

        var result = await connection.ExecuteScalarAsync<int>(
            "[dbo].[ChangeFriendshipType]",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        switch (result)
        {
            case 0:
                ChangeFriendshipTypeLogMessages.LogSuccessfulChange(logger, request);
                break;
            case 1:
                ChangeFriendshipTypeLogMessages.LogOneOrBothUsersDoNotExist(logger, result, request.UserId, request.FriendId);
                break;
            case 2:
                ChangeFriendshipTypeLogMessages.LogUnexpectedError(logger, result, request);
                break;
            default:
                ChangeFriendshipTypeLogMessages.LogUnhandledErrorCode(logger, result, request);
                break;
        }
    }
}