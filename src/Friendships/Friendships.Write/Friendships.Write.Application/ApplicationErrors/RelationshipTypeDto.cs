using ErrorOr;

namespace Friendships.Application.ApplicationErrors;

public static partial class Errors
{
    public static partial class Dto
    {
        public static class FriendshipLevelDto
        {
            public static readonly Error FriendshipLevelNotFound = Error.NotFound(
                "Dto.FriendshipLevelDto.FriendshipLevelNotFound",
                "Relationship type between users not found.");
        }
    }
}