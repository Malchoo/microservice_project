using MediatR;
using SharedKernel.IntegrationEvents.Friendships.Friendships.Write;
using SharedKernel.IntegrationEvents.User;
using System.Text.Json.Serialization;

namespace SharedKernel.IntegrationEvents;

[JsonDerivedType(typeof(UserCreatedIntegrationEvent), typeDiscriminator: nameof(UserCreatedIntegrationEvent))]
[JsonDerivedType(typeof(UsernameChangedIntegrationEvent), typeDiscriminator: nameof(UsernameChangedIntegrationEvent))]
[JsonDerivedType(typeof(FirstNameChangedIntegrationEvent), typeDiscriminator: nameof(FirstNameChangedIntegrationEvent))]
[JsonDerivedType(typeof(MiddleNameChangedIntegrationEvent), typeDiscriminator: nameof(MiddleNameChangedIntegrationEvent))]
[JsonDerivedType(typeof(LastNameChangedIntegrationEvent), typeDiscriminator: nameof(LastNameChangedIntegrationEvent))]
[JsonDerivedType(typeof(EmailChangedIntegrationEvent), typeDiscriminator: nameof(EmailChangedIntegrationEvent))]
[JsonDerivedType(typeof(MobileNumberChangedIntegrationEvent), typeDiscriminator: nameof(MobileNumberChangedIntegrationEvent))]
[JsonDerivedType(typeof(UserBlockedByAdminIntegrationEvent), typeDiscriminator: nameof(UserBlockedByAdminIntegrationEvent))]
[JsonDerivedType(typeof(UserUnblockedByAdminIntegrationEvent), typeDiscriminator: nameof(UserUnblockedByAdminIntegrationEvent))]
[JsonDerivedType(typeof(TitleChangedIntegrationEvent), typeDiscriminator: nameof(TitleChangedIntegrationEvent))]
[JsonDerivedType(typeof(LanguageChangedIntegrationEvent), typeDiscriminator: nameof(LanguageChangedIntegrationEvent))]
[JsonDerivedType(typeof(ThemeChangedIntegrationEvent), typeDiscriminator: nameof(ThemeChangedIntegrationEvent))]
[JsonDerivedType(typeof(FriendshipCreatedIntegrationEvent), typeDiscriminator: nameof(FriendshipCreatedIntegrationEvent))]

public interface IIntegrationEvent : INotification { }