using Friendships.Write.Domain.Entities;
using Friendships.Write.Domain.Ids;
using Friendships.Write.Domain.ValueObjects;
using Friendships.Write.Infrastructure.Persistence.Configurations.Constants;
using Friendships.Write.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Friendships.Write.Infrastructure.Persistence.Configurations;

public class FriendshipListConfiguration : IEntityTypeConfiguration<FriendshipList>
{
    public void Configure(EntityTypeBuilder<FriendshipList> modelBuilder)
    {
        modelBuilder.ToTable(TableNames.FriendshipList);

        modelBuilder.HasKey(fl => fl.Id);


        modelBuilder.Property(fl => fl.Id)
                    .HasColumnName(ColumnNames.UserId)
                    .HasConversion(
                        userId => userId.Value,
                        dbId => new UserId(dbId))
                    .HasColumnType(ColumTypes.UniqueIdentifier)
                    .ValueGeneratedNever()
                    .IsRequired();

        modelBuilder.ComplexProperty<Settings>("Settings",
           d =>
           {
               d.Property(f => f.IsDeleted)
                    .HasColumnName(ColumnNames.IsDeleted)
                    .HasDefaultValue(IsDeleted.No)
                    .HasConversion(
                        isDeleted => isDeleted.Value,
                        dbValue => new IsDeleted(dbValue))
                    .HasColumnType(ColumTypes.Boolean)
                    .IsRequired();
           });

        modelBuilder.ComplexProperty<Settings>("Settings",
            b =>
            {
                b.Property(f => f.IsBlockedByAdmin)
                    .HasColumnName(ColumnNames.IsBlockedByAdmin)
                    .HasDefaultValue(IsBlockedByAdmin.No)
                    .HasConversion(
                        isBlockedByAdmin => isBlockedByAdmin.Value,
                        dbValue => new IsBlockedByAdmin(dbValue))
                    .HasColumnType(ColumTypes.Boolean)
                    .IsRequired();
            });

        modelBuilder.OwnsOne<FriendshipCollection>("_activeFriendships",
            af =>
            {
                af.Property<Dictionary<FriendId, Friendship>>("_friendships")
                    .HasColumnName("ActiveFriendships")
                    .HasConversion(new CustomDictionaryConverter())
                    .HasColumnType("nvarchar(max)")
                    .Metadata.SetValueComparer(new FriendshipCollectionComparer());
            });

        modelBuilder.OwnsOne<FriendshipCollection>("_endedFriendships",
            ef =>
            {
                ef.Property<Dictionary<FriendId, Friendship>>("_friendships")
                    .HasColumnName("EndedFriendships")
                    .HasConversion(new CustomDictionaryConverter())
                    .HasColumnType("nvarchar(max)")
                    .Metadata.SetValueComparer(new FriendshipCollectionComparer());
            });

        modelBuilder.Property<UniqueIdCollection<InvitationId>>("_invitationIds")
            .HasConversion(new UniqueIdCollectionConverter<InvitationId>())
            .Metadata.SetValueComparer(new UniqueIdCollectionComparer<InvitationId>());

        modelBuilder.Property<UniqueIdCollection<BlockedId>>("_blockedIds")
            .HasConversion(new UniqueIdCollectionConverter<BlockedId>())
            .Metadata.SetValueComparer(new UniqueIdCollectionComparer<BlockedId>());
    }
}