using ErrorOr;
using Users.Domain.DomainEvents;
using Users.Domain.Enums;
using Users.Domain.Errors;
using Users.Domain.Ids;
using Users.Domain.Primitives;
using Users.Domain.Validators.Entities;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Entities;

public class User : Aggregate<UserId>
{
    private readonly List<Error> _errors = [];

    private User(
        UserId userId,
        Profile profile,
        Contacts contacts,
        Settings settings,
        Preferences preferences)
    {
        Id = userId;
        Profile = profile;
        Contacts = contacts;
        Settings = settings;
        Preferences = preferences;
    }
    public Profile Profile { get; private set; } = default!;
    public Contacts Contacts { get; private set; } = null!;
    public Settings Settings { get; private set; } = null!;
    public Preferences Preferences { get; private set; } = null!;

    public static ErrorOr<User> Create(
        RegistrationId registrationId,
        Username username,
        FirstName firstName,
        MiddleName middleName,
        LastName lastName,
        Email email,
        MobileNumber mobileNumber,
        Currency? currency,
        TwoFactorAuth? twoFactorAuth,
        Title? title,
        Language? language,
        Theme? theme)
    {
        var userId = UserId.CreateUnique();

        var profileResult = Profile.Create(registrationId, username, firstName, middleName, lastName);

        if (profileResult.IsError)
            return profileResult.Errors;

        var settings = Settings.Create();
        if (settings.IsError)
            return settings.Errors;

        var contacts = Contacts.Create(email, mobileNumber);

        if (contacts.IsError)
            return contacts.Errors;

        var preferences = Preferences.Create(currency, twoFactorAuth, title, language, theme);

        if (preferences.IsError)
            return preferences.Errors;

        var user = new User(userId, profileResult.Value, contacts.Value, settings.Value, preferences.Value);

        var validationResult = new UserValidator().Validate(user);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.ErrorCode,
                    description: validationFailure.ErrorMessage))
                .ToList();

            return errors;
        }

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user));

        return user;
    }

    public ErrorOr<User> UpdateUsername(Username username)
    {
        return IfNotBlockedByAdmin(() =>
        {
            var usernameChangeResult = Profile.ChangeUsername(username);

            if (usernameChangeResult.IsError)
                return usernameChangeResult.Errors;

            RaiseDomainEvent(new UsernameChangedDomainEvent(this));

            return this;
        });
    }

    public ErrorOr<User> UpdateFirstName(FirstName firstName)
    {
        return IfNotBlockedByAdmin(() =>
        {
            var firstNameChangeResult = Profile.ChangeFirstName(firstName);

            if (firstNameChangeResult.IsError)
                return firstNameChangeResult.Errors;

            RaiseDomainEvent(new FirstNameChangedDomainEvent(this));

            return this;
        });
    }

    public ErrorOr<User> UpdateMiddleName(MiddleName middleName)
    {
        return IfNotBlockedByAdmin(() =>
        {
            var middleNameChangeResult = Profile.ChangeMiddleName(middleName);

            if (middleNameChangeResult.IsError)
                return middleNameChangeResult.Errors;

            RaiseDomainEvent(new MiddleNameChangedDomainEvent(this));
            return this;
        });
    }

    public ErrorOr<User> UpdateLastName(LastName lastName)
    {
        return IfNotBlockedByAdmin(() =>
        {
            var lastNameChangeResult = Profile.ChangeLastName(lastName);

            if (lastNameChangeResult.IsError)
                return lastNameChangeResult.Errors;

            RaiseDomainEvent(new LastNameChangedDomainEvent(this));

            return this;
        });
    }

    public ErrorOr<User> ChangeEmail(Email email)
    {
        return IfNotBlockedByAdmin(() =>
        {
            var emailChangeResult = Contacts.ChangeEmail(email);

            if (emailChangeResult.IsError)
                return emailChangeResult.Errors;

            RaiseDomainEvent(new EmailChangedDomainEvent(this));

            return this;
        });
    }

    public ErrorOr<User> ChangeMobileNumber(MobileNumber mobileNumber)
    {
        return IfNotBlockedByAdmin(() =>
        {
            var mobileNumberChangedResult = Contacts.ChangeMobileNumber(mobileNumber);

            if (mobileNumberChangedResult.IsError)
                return mobileNumberChangedResult.Errors;

            RaiseDomainEvent(new MobileNumberChangedDomainEvent(this));

            return this;
        });
    }

    public ErrorOr<User> BlockByAdmin()
    {
        var blockByAdminResult = Settings.BlockByAdmin();

        if (blockByAdminResult.IsError)
            return blockByAdminResult.Errors;

        RaiseDomainEvent(new UserBlockedByAdminDomainEvent(this));

        return this;
    }

    public ErrorOr<User> UnblockByAdmin()
    {
        var unblockByAdminResult = Settings.UnblockByAdmin();

        if (unblockByAdminResult.IsError)
            return unblockByAdminResult.Errors;

        RaiseDomainEvent(new UserUnblockedByAdminDomainEvent(this));
        return this;
    }


    public ErrorOr<User> UpdateTitle(Title title)
    {
        return IfNotBlockedByAdmin(() =>
        {
            var updateTitleResult = Preferences.ChangeTitle(title);

            if (updateTitleResult.IsError)
                return updateTitleResult.Errors;

            RaiseDomainEvent(new TitleChangedDomainEvent(this));
            return this;
        });
    }

    public ErrorOr<User> UpdateLanguage(Language language)
    {
        return IfNotBlockedByAdmin(() =>
        {
            var updateLanguageResult = Preferences.ChangeLanguage(language);

            if (updateLanguageResult.IsError)
                return updateLanguageResult.Errors;

            RaiseDomainEvent(new LanguageChangedDomainEvent(this));
            return this;
        });
    }

    public ErrorOr<User> UpdateTheme(Theme theme)
    {
        return IfNotBlockedByAdmin(() =>
        {
            var updateThemeResult = Preferences.ChangeTheme(theme);

            if (updateThemeResult.IsError)
                return updateThemeResult.Errors;

            RaiseDomainEvent(new ThemeChangedDomainEvent(this));
            return this;
        });
    }

    private ErrorOr<User> IfNotBlockedByAdmin(Func<ErrorOr<User>> action)
    {
        if (Settings.IsBlockedByAdmin.Value)
            return UserErrors.BlockedByAdmin;

        return action();
    }

    private User()
    {
    }
}