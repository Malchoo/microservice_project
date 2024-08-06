﻿using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Entities;

namespace Friendships.Write.Domain.DomainEvents;

public sealed record DeleteAggregateDomainEvent(FriendshipList FriendshipList) : IDomainEvent;