using ErrorOr;

namespace Friendships.Application.ApplicationErrors
{
    public static partial class Errors
    {
        public static class General
        {
            public static Error ServerError(string message) => Error.Unexpected(
                code: "General.ServerError",
                description: $"Unexpected error with message: " +
                $"'{message}'.");

        }
    }
}