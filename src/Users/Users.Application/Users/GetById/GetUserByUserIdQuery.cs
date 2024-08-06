using ErrorOr;
using MediatR;

namespace Users.Application.Users.GetById;

public sealed record GetUserByUserIdQuery(Guid UserId) : IRequest<ErrorOr<GetUserByUserByIdResult>>;