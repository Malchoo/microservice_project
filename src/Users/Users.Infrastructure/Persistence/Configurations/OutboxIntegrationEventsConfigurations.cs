using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Infrastructure.IntegrationEvents;
using Users.Infrastructure.Persistence.Configurations.Constants;

namespace Users.Infrastructure.Persistence.Configurations;

public class OutboxIntegrationEventConfigurations : IEntityTypeConfiguration<OutboxIntegrationEvent>
{
    public void Configure(EntityTypeBuilder<OutboxIntegrationEvent> builder)
    {
        builder
            .ToTable(TableNames.OutboxIntegrationEventsConfigurations);

        builder
            .Property<int>(ColumnNames.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasKey(ColumnNames.Id);

        builder
            .Property(o => o.EventName);

        builder
            .Property(o => o.EventContent);
    }
}
