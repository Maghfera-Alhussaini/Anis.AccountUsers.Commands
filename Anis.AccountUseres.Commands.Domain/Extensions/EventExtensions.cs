using Anis.AccountUsers.Commands.Domain.Commands;
using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Domain.Events.DataTypes;

namespace Anis.AccountUsers.Commands.Domain.Extensions
{
    public static class EventExtensions
    {
        public static UserAssignedToAccount ToEvent(this IAssignUserToAccountCommand command, int sequence = 1)
            => new(
                aggregateId: command.AccountId,
                userId: command.UserId.ToString(),
                data: new UserAssignedToAccountData(command.UserId),
                sequence: sequence);
        public static UserDeletedFromAccount ToEvent(this IDeleteUserFromAccountCommand command, int sequence = 1)
           => new(
               aggregateId: command.AccountId,
               userId: command.UserId.ToString(),
               data: new UserDeletedFromAccountData(command.UserId),
               sequence: sequence);
    }
}
