using Contracts.Abstractions.Messages;
using Contracts.Boundaries.Cataloging.Catalog;
using Domain.Abstractions.Aggregates;
using Domain.Enumerations;
using Domain.ValueObjects;
using Version = Domain.ValueObjects.Version;

namespace Domain.Aggregates.Catalogs;

public class Catalog : AggregateRoot<CatalogId>
{
    public AppId AppId { get; private set; } = AppId.Undefined;
    public CatalogStatus Status { get; private set; } = CatalogStatus.Empty;
    public Title Title { get; private set; } = Title.Undefined;
    public Description Description { get; private set; } = Description.Undefined;

    public static Catalog Create(AppId appId, Title title, Description description)
    {
        Catalog catalog = new();
        DomainEvent.CatalogCreated @event = new(catalog.Id, appId, title, description, Version.Initial);
        catalog.RaiseEvent(@event);
        return catalog;
    }

    public void Activate()
    {
        if (Status is CatalogActive)
            throw new InvalidOperationException("Catalog is already active.");

        if (Status is CatalogEmpty)
            throw new InvalidOperationException("Catalog is empty.");

        RaiseEvent(new DomainEvent.CatalogActivated(Id, CatalogStatus.Active, Version.Next));
    }

    public void Deactivate()
    {
        if (Status is CatalogInactive)
            throw new InvalidOperationException("Catalog is already inactive.");

        RaiseEvent(new DomainEvent.CatalogInactivated(Id, CatalogStatus.Inactive, Version.Next));
    }

    public void ChangeCatalogTitle(Title title)
        => RaiseEvent(new DomainEvent.CatalogTitleChanged(Id, title, Version.Next));

    public void ChangeDescription(Description description)
        => RaiseEvent(new DomainEvent.CatalogDescriptionChanged(Id, description, Version.Next));

    public void Delete()
        => RaiseEvent(new DomainEvent.CatalogDeleted(Id, CatalogStatus.Discarded, Version.Next));

    protected override void ApplyEvent(IDomainEvent @event) => When(@event as dynamic);

    private void When(DomainEvent.CatalogCreated @event)
    {
        Id = (CatalogId)@event.CatalogId;
        AppId = (AppId)@event.AppId;
        Title = (Title)@event.Title;
        Description = (Description)@event.Description;
        Version = (Version)@event.Version;
    }

    private void When(DomainEvent.CatalogDescriptionChanged @event)
    {
        Description = (Description)@event.Description;
        Version = (Version)@event.Version;
    }

    private void When(DomainEvent.CatalogTitleChanged @event)
    {
        Title = (Title)@event.Title;
        Version = (Version)@event.Version;
    }

    private void When(DomainEvent.CatalogActivated @event)
    {
        Status = (CatalogStatus)@event.Status;
        Version = (Version)@event.Version;
    }

    private void When(DomainEvent.CatalogInactivated @event)
    {
        Status = (CatalogStatus)@event.Status;
        Version = (Version)@event.Version;
    }

    private void When(DomainEvent.CatalogDeleted @event)
    {
        Status = (CatalogStatus)@event.Status;
        IsDeleted = true;
        Version = (Version)@event.Version;
    }
}