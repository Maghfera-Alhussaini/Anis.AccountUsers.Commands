using Anis.AccountUsers.Commands.Application.Contracts.Services.ServiceBus;
using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using Anis.AccountUsers.Commands.Grpc.Protos.Demo;
using Azure.Core;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Anis.AccountUsers.Commands.Grpc.Services
{
    public class AccountUsersDemoEventsService(IServiceBusEventSender sender) : AccountUsersDemoEvents.AccountUsersDemoEventsBase
    {
        private readonly IServiceBusEventSender _sender = sender;

        public override async Task<Empty> AssignUsers(UserRequest request, ServerCallContext context)
        {
            var @event = new UserAssignedToAccount(
                       Guid.Parse(request.AccountId),
                        Guid.NewGuid().ToString(),
                        new UserAssignedToAccountData(Guid.Parse(request.UserId)),
                        request.Sequence);

            await _sender.SendEventAsync(@event);

            return new Empty();

        }

        public async override Task<Empty> DeleteUsers(UserRequest request, ServerCallContext context)
        {
            var @event = new UserDeletedFromAccount(
                    Guid.Parse(request.AccountId),
                    Guid.NewGuid().ToString(),
                    new UserDeletedFromAccountData(Guid.Parse(request.UserId)),
                    request.Sequence);

            await _sender.SendEventAsync(@event);

            return new Empty();
        }
    }
}
