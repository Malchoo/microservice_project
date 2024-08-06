using ErrorOr;
using Users.Domain.Errors.Profile;
using Users.Domain.Ids;
using Users.Domain.Validators.ValueObjects;

namespace Users.Domain.ValueObjects.ReferenceType
{
    public sealed record Profile
    {
        private Profile(
            RegistrationId registrationId,
            Username username,
            FirstName firstName,
            MiddleName middleName,
            LastName lastName)
        {
            RegistrationId = registrationId;
            Username = username;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }

        public RegistrationId RegistrationId { get; init; }
        public Username Username { get; init; } = null!;
        public FirstName FirstName { get; init; } = null!;
        public MiddleName MiddleName { get; init; } = null!;
        public LastName LastName { get; init; } = null!;

        internal static ErrorOr<Profile> Create(
            RegistrationId registrationId,
            Username username,
            FirstName firstName,
            MiddleName middleName,
            LastName lastName)
        {
            var profile = new Profile(registrationId, username, firstName, middleName, lastName);
            var profileResult = new ProfileValidator().Validate(profile);

            if (!profileResult.IsValid)
            {
                var errors = profileResult.Errors
                    .Select(validationFailure => Error.Validation(
                        code: validationFailure.ErrorCode,
                        description: validationFailure.ErrorMessage))
                    .ToList();

                return errors;
            }

            return profile;
        }

        internal ErrorOr<Profile> ChangeUsername(Username newUsername)
            => ChangeProperty(
                Username,
                newUsername,
                ProfileErrors.UsernameNotChanged,
                username => this with { Username = username });

        internal ErrorOr<Profile> ChangeFirstName(FirstName newFirstName)
            => ChangeProperty(
                FirstName,
                newFirstName,
                ProfileErrors.FirstNameNotChanged,
                firstName => this with { FirstName = firstName });

        internal ErrorOr<Profile> ChangeMiddleName(MiddleName newMiddleName)
            => ChangeProperty(
                MiddleName,
                newMiddleName,
                ProfileErrors.MiddleNameNotChanged,
                middleName => this with { MiddleName = middleName });

        internal ErrorOr<Profile> ChangeLastName(LastName newLastName)
            => ChangeProperty(
                LastName,
                newLastName,
                ProfileErrors.LastNameNotChanged,
                lastName => this with { LastName = lastName });

        private static ErrorOr<Profile> ChangeProperty<T>(
            T currentValue,
            T newValue,
            Func<T, Error> errorFunc,
            Func<T, Profile> changeFunc)
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return errorFunc(newValue);

            return changeFunc(newValue);
        }

        private Profile()
        {
        }
    }
}