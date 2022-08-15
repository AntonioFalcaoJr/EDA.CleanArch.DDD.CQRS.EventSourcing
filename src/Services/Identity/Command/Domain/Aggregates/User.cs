﻿using Domain.Abstractions.Aggregates;
using Contracts.Abstractions.Messages;
using Contracts.Services.Identity;

namespace Domain.Aggregates;

public class User : AggregateRoot<Guid, UserValidator>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string PasswordConfirmation { get; private set; }

    public override void Handle(ICommand command)
        => Handle(command as dynamic);

    private void Handle(Command.RegisterUser cmd)
        => Raise(new DomainEvent.UserRegistered(Guid.NewGuid(), cmd.FirstName, cmd.LastName, cmd.Email, cmd.Password, cmd.PasswordConfirmation));

    private void Handle(Command.ChangePassword cmd)
        => Raise(new DomainEvent.UserPasswordChanged(cmd.UserId, cmd.NewPassword, cmd.NewPasswordConfirmation));

    private void Handle(Command.DeleteUser cmd)
        => Raise(new DomainEvent.UserDeleted(cmd.UserId));

    protected override void Apply(IEvent @event)
        => Apply(@event as dynamic);

    private void Apply(DomainEvent.UserRegistered @event)
        => (Id, FirstName, LastName, Email, Password, PasswordConfirmation) = @event;

    private void Apply(DomainEvent.UserPasswordChanged @event)
        => (_, Password, PasswordConfirmation) = @event;

    private void Apply(DomainEvent.UserDeleted _)
        => IsDeleted = true;
}