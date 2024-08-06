using ErrorOr;

namespace Friendships.Application.ApplicationErrors;

public static partial class Errors
{
    public static partial class Dto
    {
        public static class GetFriendsDto
        {
            public static readonly Error FriendsNotFound = Error.NotFound(
                "Dto.GetFriendsDto.FriendsNotFound",
                "Friends not found.");
        }
    }
}