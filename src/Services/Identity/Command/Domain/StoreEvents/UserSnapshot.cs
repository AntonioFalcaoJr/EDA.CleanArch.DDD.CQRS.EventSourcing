﻿using Domain.Abstractions.StoreEvents;
using Domain.Aggregates;

namespace Domain.StoreEvents;

public record UserSnapshot : Snapshot<Guid, User>;