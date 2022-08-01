using Application.Abstractions.EventStore;
using Domain.Aggregates;
using Domain.StoreEvents;

namespace Application.EventStore;

public interface IUserEventStoreRepository : IEventStoreRepository<User, UserStoreEvent, UserSnapshot, Guid> { }