using MediatR;
using SharedKernel.IntegrationEvents;
using SharedKernel.IntegrationEvents.User;
using System.Text.Json;
using Users.Domain.DomainEvents;

namespace Users.Infrastructure.IntegrationEvents.OutboxWriter;

public class OutboxWriterEventHandler
    : INotificationHandler<UserCreatedDomainEvent>,
    INotificationHandler<UsernameChangedDomainEvent>,
    INotificationHandler<FirstNameChangedDomainEvent>,
    INotificationHandler<MiddleNameChangedDomainEvent>,
    INotificationHandler<LastNameChangedDomainEvent>,
    INotificationHandler<EmailChangedDomainEvent>,
    INotificationHandler<MobileNumberChangedDomainEvent>,
    INotificationHandler<UserBlockedByAdminDomainEvent>,
    INotificationHandler<UserUnblockedByAdminDomainEvent>,
    INotificationHandler<TitleChangedDomainEvent>,
    INotificationHandler<LanguageChangedDomainEvent>,
    INotificationHandler<ThemeChangedDomainEvent>
{
    private readonly UserDbContext _dbContext;

    public OutboxWriterEventHandler(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new UserCreatedIntegrationEvent(
            UserId: notification.User.Id.Value);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }


    public async Task Handle(UsernameChangedDomainEvent notification, CancellationToken ct)
    {
        var intergrationEvent = new UsernameChangedIntegrationEvent(
            UserId: notification.User.Id.Value,
            Username: notification.User.Profile.Username.Value);

        await AddOutboxIntegrationEventAsync(intergrationEvent);
    }

    public async Task Handle(FirstNameChangedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new FirstNameChangedIntegrationEvent(
            UserId: notification.User.Id.Value,
            FirstName: notification.User.Profile.FirstName.Value);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(MiddleNameChangedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new MiddleNameChangedIntegrationEvent(
            UserId: notification.User.Id.Value,
            MiddleName: notification.User.Profile.MiddleName.Value);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(LastNameChangedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new LastNameChangedIntegrationEvent(
            UserId: notification.User.Id.Value,
            LastName: notification.User.Profile.LastName.Value);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(EmailChangedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new EmailChangedIntegrationEvent(
            UserId: notification.User.Id.Value,
            Email: notification.User.Contacts.Email.Value);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(MobileNumberChangedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new MobileNumberChangedIntegrationEvent(
            UserId: notification.User.Id.Value,
            MobileNumber: notification.User.Contacts.MobileNumber.Value);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(UserBlockedByAdminDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new UserBlockedByAdminIntegrationEvent(
            UserId: notification.User.Id.Value);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(UserUnblockedByAdminDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new UserUnblockedByAdminIntegrationEvent(
         UserId: notification.User.Id.Value);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(TitleChangedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new TitleChangedIntegrationEvent(
            UserId: notification.User.Id.Value,
            Title: notification.User.Preferences.Title.Name);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(LanguageChangedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new LanguageChangedIntegrationEvent(
            UserId: notification.User.Id.Value,
            Language: notification.User.Preferences.Language.Name);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(ThemeChangedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new ThemeChangedIntegrationEvent(
            UserId: notification.User.Id.Value,
            Theme: notification.User.Preferences.Theme.Name);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }
    private async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent)
    {
        await _dbContext.OutboxIntegrationEvents.AddAsync(new OutboxIntegrationEvent(
            EventName: integrationEvent.GetType().Name,
            EventContent: JsonSerializer.Serialize(integrationEvent)));

        await _dbContext.SaveChangesAsync();
    }
}
