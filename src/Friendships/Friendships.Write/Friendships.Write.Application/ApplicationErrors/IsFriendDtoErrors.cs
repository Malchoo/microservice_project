using ErrorOr;

namespace Friendships.Application.ApplicationErrors;
public static partial class Errors
{
    public static partial class Dto
    {
        public static class IsFriendDtoErrors
        {
            public static readonly Error UserOrFriendNotFound = Error.NotFound(
                "Dto.IsFriendDto.UserOrFriendNotFound",
                "User or friend not found.");
        }
    }
}