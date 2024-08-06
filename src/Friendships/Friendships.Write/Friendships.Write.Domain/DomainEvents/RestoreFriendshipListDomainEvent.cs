﻿using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Entities;

namespace Friendships.Write.Domain.DomainEvents;

public sealed record RestoreAggregateDomainEvent(FriendshipList FriendshipList) : IDomainEvent;