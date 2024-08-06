using ErrorOr;
using MediatR;
using Users.Domain.Contracts;
using Users.Domain.Errors;
using Users.Domain.Ids;

namespace Users.Application.Users.GetById;

public sealed class GetByUserIdQueryHandler : IRequestHandler<GetUserByUserIdQuery, ErrorOr<GetUserByUserByIdResult>>
{
    private readonly IUserRepository _userRepository;

    public GetByUserIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<GetUserByUserByIdResult>> Handle(GetUserByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);

        var userResult = await _userRepository.GetUserByUserIdAsync(userId);

        if (userResult is null)
            return UserErrors.UserNotFound;

        return new GetUserByUserByIdResult(userResult);
    }
}
