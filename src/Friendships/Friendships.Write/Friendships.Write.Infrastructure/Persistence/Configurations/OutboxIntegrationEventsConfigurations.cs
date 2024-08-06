using Friendships.Write.Infrastructure.IntegrationEvents;
using Friendships.Write.Infrastructure.Persistence.Configurations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Friendships.Write.Infrastructure.Persistence.Configurations;

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
