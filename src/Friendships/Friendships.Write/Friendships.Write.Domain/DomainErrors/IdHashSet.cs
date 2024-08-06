using ErrorOr;

namespace Friendships.Domain.DomainErrors;

public static partial class Errors
{
    public static partial class ValueObjects
    {
        public sealed class UniqueIdCollection
        {
            public static readonly Error NotFound = Error.NotFound(
                code: "ValueObjects.UniqueIdCollection.NotFound",
                description: "Id not found in the UniqueIdCollection.");

            public static readonly Error AlreadyInUniqueIdCollection = Error.Conflict(
                code: "ValueObjects.UniqueIdCollection.AlreadyInUniqueIdCollection",
                description: "Id is already in the UniqueIdCollection. Duplicated Id.");

            public static readonly Error NotEmptyGuid = Error.Validation(
                code: "ValueObjects.UniqueIdCollection.NotEmptyGuid",
                description: "UniqueIdCollection cannot contain an empty Guid.");

            public static Error TypeMismatch(string expectedType, string actualType) => Error.Validation(
                code: "ValueObjects.UniqueIdCollection.TypeMismatch",
                description: $"UniqueIdCollection cannot contain Ids of type {actualType}. Expected type is {expectedType}.");
        }
    }
}