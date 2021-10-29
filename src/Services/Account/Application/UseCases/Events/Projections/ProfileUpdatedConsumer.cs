using System.Threading.Tasks;
using Application.EventSourcing.EventStore;
using Application.EventSourcing.Projections;
using MassTransit;
using ProfileUpdatedEvent = Messages.Accounts.Events.ProfileUpdated;

namespace Application.UseCases.Events.Projections
{
    public class ProfileUpdatedConsumer : IConsumer<ProfileUpdatedEvent>
    {
        private readonly IAccountEventStoreService _eventStoreService;
        private readonly IAccountProjectionsService _projectionsService;

        public ProfileUpdatedConsumer(IAccountEventStoreService eventStoreService, IAccountProjectionsService projectionsService)
        {
            _eventStoreService = eventStoreService;
            _projectionsService = projectionsService;
        }

        public async Task Consume(ConsumeContext<ProfileUpdatedEvent> context)
        {
            var account = await _eventStoreService.LoadAggregateFromStreamAsync(context.Message.AccountId, context.CancellationToken);

            var accountDetails = new AccountDetailsProjection
            {
                Id = account.Id,
                Profile = new ()
                {
                    Birthdate = account.Profile.Birthdate,
                    Email = account.Profile.Email,
                    FirstName = account.Profile.FirstName,
                    LastName = account.Profile.LastName
                }
            };

            await _projectionsService.ProjectAsync(accountDetails, context.CancellationToken);
        }
    }
}