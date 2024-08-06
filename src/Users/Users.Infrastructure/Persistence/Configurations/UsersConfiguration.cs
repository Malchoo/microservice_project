using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;
using Users.Domain.Enums;
using Users.Domain.Ids;
using Users.Domain.ValueObjects.ReferenceType;
using Users.Domain.ValueObjects.ValueType;
using Users.Infrastructure.Persistence.Configurations.Constants;

public class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users);

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName(ColumnNames.UserId)
            .HasConversion(
                userId => userId.Value,
                dbId => new UserId(dbId))
            .HasColumnType(ColumTypes.UniqueIdentifier);

        builder.ComplexProperty(u => u.Profile, 
            registrationIdBuilder =>
           {
               registrationIdBuilder.Property(ri => ri.RegistrationId)
                   .HasColumnName(ColumnNames.RegistrationId)
                   .HasConversion(
                       registrationId => registrationId.Value,
                       dbRegistrationId => new RegistrationId(dbRegistrationId))
                   .HasColumnType(ColumTypes.UniqueIdentifier)
                   .IsRequired();
           });

        builder.ComplexProperty(u => u.Profile, 
            usernameBuilder =>
            {
                usernameBuilder.Property(profile => profile.Username)
                    .HasColumnName(ColumnNames.Username)
                    .HasConversion(
                        username => username.Value,
                        dbUsername => Username.Create(dbUsername).Value)
                    .HasMaxLength(Username.MaxLength)
                    .IsRequired();
            });

        builder.ComplexProperty(u => u.Profile, 
            firstNameBuilder =>
            {
                firstNameBuilder.Property(profile => profile.FirstName)
                    .HasColumnName(ColumnNames.FirstName)
                    .HasConversion(
                        firstName => firstName.Value,
                        dbFirstName => FirstName.Create(dbFirstName).Value)
                    .HasMaxLength(FirstName.MaxLength)
                    .IsRequired();
            });

        builder.ComplexProperty(u => u.Profile, 
            middleNameBuilder =>
            {
                middleNameBuilder.Property(profile => profile.MiddleName)
                    .HasColumnName(ColumnNames.MiddleName)
                    .HasConversion(
                        middleName => middleName.Value,
                        dbMiddleName => MiddleName.Create(dbMiddleName).Value)
                    .HasMaxLength(MiddleName.MaxLength)
                    .IsRequired();
            });

        builder.ComplexProperty(u => u.Profile, 
            lastNameBuilder =>
            {
                lastNameBuilder.Property(profile => profile.LastName)
                    .HasColumnName(ColumnNames.LastName)
                    .HasConversion(
                        lastName => lastName.Value,
                        dbLastName => LastName.Create(dbLastName).Value)
                    .HasMaxLength(LastName.MaxLength)
                    .IsRequired();
            });

        builder.ComplexProperty(u => u.Contacts, 
            emailBuilder =>
            {
                emailBuilder.Property(contacts => contacts.Email)
                    .HasColumnName(ColumnNames.Email)
                    .HasConversion(
                        email => email.Value,
                        dbEmail => Email.Create(dbEmail).Value)
                    .HasMaxLength(Email.MaxLength)
                    .IsRequired();

                //ToDo пробвай с .IsSparse(), а не с .IsRequired()
            });

        builder.ComplexProperty(u => u.Contacts, 
            mobileNumberBuilder =>
            {
                mobileNumberBuilder.Property(contacts => contacts.MobileNumber)
                    .HasColumnName(ColumnNames.MobileNumber)
                    .HasConversion(
                        mobileNumber => mobileNumber.Value,
                        dBmobileNumber => MobileNumber.Create(dBmobileNumber).Value)
                    .HasMaxLength(MobileNumber.MaxLength)
                    .IsRequired();

                //ToDo пробвай с .IsSparse(), а не с .IsRequired()
            });

        builder.ComplexProperty(u => u.Settings, 
            isVerifiedlBuilder =>
            {
                isVerifiedlBuilder.Property(settings => settings.IsVerified)
                    .HasColumnName(ColumnNames.IsVerified)
                    .HasDefaultValue(IsVerified.Yes) //TODO this shoud be IsVerified.No
                    .HasConversion(
                        isVerified => isVerified.Value,
                        dbIsVerified => new IsVerified(dbIsVerified))
                    .HasColumnType(ColumTypes.Boolean)
                    .IsRequired();
            });

        builder.ComplexProperty(u => u.Settings, 
            isBlockedByAdminBuilder =>
            {
                isBlockedByAdminBuilder.Property(settings => settings.IsBlockedByAdmin)
                    .HasColumnName(ColumnNames.IsBlockedByAdmin)
                    .HasDefaultValue(IsBlockedByAdmin.No)
                    .HasConversion(
                        isBlockedByAdmin => isBlockedByAdmin.Value,
                        dbIsBlockedByAdmin => new IsBlockedByAdmin(dbIsBlockedByAdmin))
                    .HasColumnType(ColumTypes.Boolean)
                    .IsRequired();
            });

        builder.ComplexProperty(u => u.Settings, 
            isDeletedBuilder =>
            {
                isDeletedBuilder.Property(settings => settings.IsDeleted)
                    .HasColumnName(ColumnNames.IsDeleted)
                    .HasDefaultValue(IsDeleted.No)
                    .HasConversion(
                        isDeleted => isDeleted.Value,
                        dBIsDeleted => new IsDeleted(dBIsDeleted))
                    .HasColumnType(ColumTypes.Boolean)
                    .IsRequired();
            });

        builder.ComplexProperty(u => u.Preferences, 
            currencyBuilder =>
            {
                currencyBuilder.Property(preferences => preferences.Currency)
                    .HasColumnName(ColumnNames.Currency)
                    .HasDefaultValue(Currency.BGN)
                    .HasConversion(
                        currency => currency.Value,
                        dbCurrency => Currency.FromValue(dbCurrency))
                    .HasColumnType(ColumTypes.TinyInt)
                    .IsRequired();
            });

        builder.ComplexProperty(u => u.Preferences, 
            twoFactorAuthBuilder =>
          {
              twoFactorAuthBuilder.Property(preferences => preferences.TwoFactorAuth)
                    .HasColumnName(ColumnNames.TwoFactorAuth)
                    .HasDefaultValue(TwoFactorAuth.Email)
                    .HasConversion(
                          twoFactorAuth => twoFactorAuth.Value,
                          dbTwoFactorAuth => TwoFactorAuth.FromValue(dbTwoFactorAuth))
                    .HasColumnType(ColumTypes.TinyInt)
                    .IsRequired();
          });

        builder.ComplexProperty(u => u.Preferences, 
            titleBuilder =>
            {
                titleBuilder.Property(preferences => preferences.Title)
                    .HasColumnName(ColumnNames.Title)
                    .HasDefaultValue(Title.None)
                    .HasConversion(
                        title => title.Value,
                        dbTitle => Title.FromValue(dbTitle))
                    .HasColumnType(ColumTypes.TinyInt)
                    .IsRequired();
            });

        builder.ComplexProperty(u => u.Preferences, 
            languageBuilder =>
            {
                languageBuilder.Property(preferences => preferences.Language)
                    .HasColumnName(ColumnNames.Language)
                    .HasDefaultValue(Language.BG)
                    .HasConversion(
                        language => language.Value,
                        dbLanguage => Language.FromValue(dbLanguage))
                    .HasColumnType(ColumTypes.TinyInt)
                    .IsRequired();
            });

        builder.ComplexProperty(u => u.Preferences, 
            themeBuilder =>
            {
                themeBuilder.Property(preferences => preferences.Theme)
                    .HasColumnName(ColumnNames.Theme)
                    .HasDefaultValue(Theme.Light)
                    .HasConversion(
                        theme => theme.Value,
                        dbTheme => Theme.FromValue(dbTheme))
                    .HasColumnType(ColumTypes.TinyInt)
                    .IsRequired();
            });
    }
}