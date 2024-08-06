using ErrorOr;

namespace Users.Domain.Errors.Profile;

public static class FullNameErrors
{
    public static readonly Error NotAllComponentsProvided = Error.Validation(
        code: "FullNameErrors.NotAllComponentsProvided",
        description: "All components of full name must be provided.");
}