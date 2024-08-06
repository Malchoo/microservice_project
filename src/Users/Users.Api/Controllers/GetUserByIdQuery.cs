using ErrorOr;
using MediatR;

namespace Users.Api.Controllers;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<ErrorOr<GetUserByIdResult>>;